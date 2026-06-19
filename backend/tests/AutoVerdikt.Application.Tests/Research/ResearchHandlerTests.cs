using AutoVerdikt.Application.Abstractions;
using AutoVerdikt.Application.Research;
using AutoVerdikt.Application.Research.Create;
using AutoVerdikt.Application.Research.Create.StartForm;
using AutoVerdikt.Application.Research.Create.StartText;
using AutoVerdikt.Application.Research.Delete;
using AutoVerdikt.Application.Research.Errors;
using AutoVerdikt.Application.Research.Get;
using AutoVerdikt.Application.Research.Rename;
using AutoVerdikt.Domain.Research;
using FluentResults;
using Mediator;
using Moq;
using Shouldly;

namespace AutoVerdikt.Application.Tests.Research;

public class ResearchHandlerTests
{
    private readonly Mock<IResearchRepository> _repository = new();
    private readonly Mock<ICurrentUserContext> _currentUser = new();

    [Fact]
    public async Task StartForm_PersistsRecordWithPendingStatusAndAuthId()
    {
        _currentUser.Setup(c => c.AuthId).Returns("auth_abc");
        _repository
            .Setup(r => r.CreateAsync(It.IsAny<ResearchRecord>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());

        var car = new CarData
        {
            Make = "Volkswagen",
            Model = "Golf",
            Year = 2018,
            MileageKm = 87200,
            Price = 42900
        };

        var result = await new StartFormResearchCommandHandler(
            _repository.Object,
            _currentUser.Object,
            TimeProvider.System).Handle(
            new StartFormResearchCommand(car),
            CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Status.ShouldBe(ResearchStatus.Pending);
        result.Value.AuthId.ShouldBe("auth_abc");
        _repository.Verify(
            r => r.CreateAsync(It.Is<ResearchRecord>(x => x.Status == ResearchStatus.Pending && x.AuthId == "auth_abc"), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task StartText_ReturnsInputMethodNotSupportedError()
    {
        var result = await new StartTextResearchCommandHandler(
            _repository.Object,
            _currentUser.Object,
            TimeProvider.System).Handle(
            new StartTextResearchCommand(),
            CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        result.Errors[0].ShouldBeOfType<InputMethodNotSupportedError>();
        _repository.Verify(
            r => r.CreateAsync(It.IsAny<ResearchRecord>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateResearch_DispatchesStartFormResearchCommand()
    {
        var car = new CarData { Make = "Volkswagen", Model = "Golf", Year = 2018 };
        var factory = new Mock<IStartResearchCommandFactory>();
        var mediator = new Mock<IMediator>();
        var expected = Result.Ok(ResearchRecord.Create(
            "auth_abc",
            InputMethod.Form,
            car,
            null,
            null,
            TimeProvider.System));

        factory
            .Setup(f => f.Create(It.Is<CreateResearchCommand>(c => c.InputMethod == InputMethod.Form && c.Car == car)))
            .Returns(Result.Ok<IRequest<Result<ResearchRecord>>>(new StartFormResearchCommand(car)));
        mediator
            .Setup(m => m.Send(It.IsAny<StartFormResearchCommand>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<Result<ResearchRecord>>(expected));

        var result = await new CreateResearchCommandHandler(factory.Object, mediator.Object).Handle(
            new CreateResearchCommand(InputMethod.Form, car),
            CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        factory.Verify(f => f.Create(It.IsAny<CreateResearchCommand>()), Times.Once);
        mediator.Verify(m => m.Send(It.IsAny<StartFormResearchCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Get_NotFound_ReturnsResearchNotFoundError()
    {
        _currentUser.Setup(c => c.AuthId).Returns("auth_abc");
        _repository
            .Setup(r => r.GetByIdAndAuthIdAsync(It.IsAny<Guid>(), "auth_abc", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok<ResearchRecord?>(null));

        var result = await new GetResearchQueryHandler(_repository.Object, _currentUser.Object).Handle(
            new GetResearchQuery(Guid.CreateVersion7()),
            CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        result.Errors[0].ShouldBeOfType<ResearchNotFoundError>();
    }

    [Fact]
    public async Task Rename_ManualName_IsPreservedAndUsesAuthId()
    {
        var id = Guid.CreateVersion7();
        var existing = ResearchRecord.Create(
            "auth_abc",
            InputMethod.Form,
            new CarData { Make = "Volkswagen", Model = "Golf", Year = 2018, MileageKm = 1, Price = 1 },
            null,
            null,
            TimeProvider.System) with
        { Id = id };

        _currentUser.Setup(c => c.AuthId).Returns("auth_abc");
        _repository
            .Setup(r => r.GetByIdAndAuthIdAsync(id, "auth_abc", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok<ResearchRecord?>(existing));
        _repository
            .Setup(r => r.UpdateAsync(It.IsAny<ResearchRecord>(), "auth_abc", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());

        var result = await new RenameResearchCommandHandler(
            _repository.Object,
            _currentUser.Object,
            TimeProvider.System).Handle(
            new RenameResearchCommand(id, "Custom Name"),
            CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Name.ShouldBe("Custom Name");
        result.Value.IsNameManual.ShouldBeTrue();
        _repository.Verify(r => r.UpdateAsync(It.IsAny<ResearchRecord>(), "auth_abc", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Delete_UsesCurrentUserAuthId()
    {
        var id = Guid.CreateVersion7();
        _currentUser.Setup(c => c.AuthId).Returns("auth_abc");
        _repository
            .Setup(r => r.DeleteAsync(id, "auth_abc", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());

        var result = await new DeleteResearchCommandHandler(_repository.Object, _currentUser.Object).Handle(
            new DeleteResearchCommand(id),
            CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        _repository.Verify(r => r.DeleteAsync(id, "auth_abc", It.IsAny<CancellationToken>()), Times.Once);
    }
}
