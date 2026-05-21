using AutoVerdikt.Application.Abstractions;
using AutoVerdikt.Application.Users;
using AutoVerdikt.Application.Users.Register;
using AutoVerdikt.Domain.Users;
using Moq;
using Shouldly;

namespace AutoVerdikt.Application.Tests.Users.Register;

public class UserRegisterCommandHandlerTests
{
    private readonly Mock<IUserRepository> _repository = new();
    private readonly Mock<ICurrentUserContext> _currentUser = new();

    private UserRegisterCommandHandler CreateHandler() =>
        new(_repository.Object, _currentUser.Object, TimeProvider.System);

    [Fact]
    public async Task Handle_NewUser_ReturnsOkWithCorrectProperties()
    {
        _currentUser.Setup(c => c.AuthId).Returns("auth_abc123");
        _repository
            .Setup(r => r.ExistsByAuthIdAsync("auth_abc123", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _repository
            .Setup(r => r.CreateAsync(It.IsAny<UserAccount>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await CreateHandler().Handle(
            new UserRegisterCommand("Alice", "alice@example.com"), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.AuthId.ShouldBe("auth_abc123");
        result.Value.Name.ShouldBe("Alice");
        result.Value.Email.ShouldBe("alice@example.com");
        result.Value.Id.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public async Task Handle_NewUser_PersistsUserExactlyOnce()
    {
        _currentUser.Setup(c => c.AuthId).Returns("auth_abc123");
        _repository
            .Setup(r => r.ExistsByAuthIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _repository
            .Setup(r => r.CreateAsync(It.IsAny<UserAccount>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await CreateHandler().Handle(
            new UserRegisterCommand("Alice", "alice@example.com"), CancellationToken.None);

        _repository.Verify(
            r => r.CreateAsync(It.IsAny<UserAccount>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_AlreadyRegistered_ReturnsFailWithMessage()
    {
        _currentUser.Setup(c => c.AuthId).Returns("auth_abc123");
        _repository
            .Setup(r => r.ExistsByAuthIdAsync("auth_abc123", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await CreateHandler().Handle(
            new UserRegisterCommand("Alice", "alice@example.com"), CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].Message.ShouldBe("User is already registered.");
    }

    [Fact]
    public async Task Handle_AlreadyRegistered_DoesNotPersist()
    {
        _currentUser.Setup(c => c.AuthId).Returns("auth_abc123");
        _repository
            .Setup(r => r.ExistsByAuthIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        await CreateHandler().Handle(
            new UserRegisterCommand("Alice", "alice@example.com"), CancellationToken.None);

        _repository.Verify(
            r => r.CreateAsync(It.IsAny<UserAccount>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
