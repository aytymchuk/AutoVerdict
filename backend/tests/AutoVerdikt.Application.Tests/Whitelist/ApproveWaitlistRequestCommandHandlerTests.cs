using AutoVerdikt.Application.Email;
using AutoVerdikt.Application.Users;
using AutoVerdikt.Application.Whitelist;
using AutoVerdikt.Application.Whitelist.Admin.Approve;
using AutoVerdikt.Domain.Whitelist;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace AutoVerdikt.Application.Tests.Whitelist;

public sealed class ApproveWaitlistRequestCommandHandlerTests
{
    private readonly Mock<IWaitlistRequestRepository> _repository = new();
    private readonly Mock<ISendGridService> _sendGrid = new();

    [Fact]
    public async Task Handle_ApprovedRequest_SendsEmailAndUpdatesStatus()
    {
        var request = WaitlistRequest.Create("auth_1", "user@example.com", "about", "pl", TimeProvider.System);
        _repository.Setup(r => r.GetByIdAsync(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(request);

        var result = await CreateHandler().Handle(new ApproveWaitlistRequestCommand(request.Id), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        _sendGrid.Verify(s => s.SendApprovalEmailAsync("user@example.com", "pl", It.IsAny<CancellationToken>()), Times.Once);
        _repository.Verify(r => r.UpdateAsync(It.Is<WaitlistRequest>(w => w.Status == WaitlistRequestStatus.Approved), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_AlreadyApproved_IsIdempotent()
    {
        var request = WaitlistRequest.Create("auth_1", "user@example.com", null, "en", TimeProvider.System);
        var approved = request.Approve(DateTimeOffset.UtcNow);
        _repository.Setup(r => r.GetByIdAsync(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(approved);

        var result = await CreateHandler().Handle(new ApproveWaitlistRequestCommand(request.Id), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        _sendGrid.Verify(s => s.SendApprovalEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _repository.Verify(r => r.UpdateAsync(It.IsAny<WaitlistRequest>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    private ApproveWaitlistRequestCommandHandler CreateHandler()
    {
        var logger = new Mock<ILogger<ApproveWaitlistRequestCommandHandler>>();
        var userRepository = new Mock<IUserRepository>();
        return new(_repository.Object, userRepository.Object, _sendGrid.Object, TimeProvider.System, logger.Object);
    }
}
