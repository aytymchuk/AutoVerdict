using AutoVerdikt.Application.Abstractions;
using AutoVerdikt.Application.Users;
using AutoVerdikt.Application.Whitelist;
using AutoVerdikt.Application.Whitelist.Errors;
using AutoVerdikt.Application.Whitelist.SubmitWaitlistRequest;
using AutoVerdikt.Domain.Users;
using Moq;
using Shouldly;

namespace AutoVerdikt.Application.Tests.Whitelist;

public sealed class SubmitWaitlistRequestCommandHandlerTests
{
    private readonly Mock<IWaitlistRequestRepository> _repository = new();
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<ICurrentUserContext> _currentUser = new();

    [Fact]
    public async Task Handle_NewRequest_ReturnsSuccess()
    {
        _currentUser.Setup(c => c.AuthId).Returns("auth_1");
        _currentUser.Setup(c => c.Email).Returns("User@Example.com");
        _currentUser.Setup(c => c.Locale).Returns("en");
        _repository.Setup(r => r.ExistsByAuthIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _repository.Setup(r => r.ExistsByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var user = UserAccount.Create("auth_1", "Alice", "user@example.com", TimeProvider.System);
        _userRepository.Setup(r => r.GetByAuthIdAsync("auth_1", It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var result = await CreateHandler().Handle(new SubmitWaitlistRequestCommand("about"), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        _repository.Verify(r => r.CreateAsync(It.IsAny<Domain.Whitelist.WaitlistRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        _userRepository.Verify(
            r => r.UpdateWhitelistStatusAsync(
                It.Is<UserAccount>(u => u.WhitelistStatus == WhitelistStatus.Requested),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_DuplicateAuthId_ReturnsAlreadySubmittedError()
    {
        _currentUser.Setup(c => c.AuthId).Returns("auth_1");
        _currentUser.Setup(c => c.Email).Returns("user@example.com");
        _currentUser.Setup(c => c.Locale).Returns("en");
        _repository.Setup(r => r.ExistsByAuthIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var result = await CreateHandler().Handle(new SubmitWaitlistRequestCommand(null), CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        result.Errors[0].ShouldBeOfType<WaitlistAlreadySubmittedError>();
    }

    private SubmitWaitlistRequestCommandHandler CreateHandler() =>
        new(_repository.Object, _userRepository.Object, _currentUser.Object, TimeProvider.System);
}
