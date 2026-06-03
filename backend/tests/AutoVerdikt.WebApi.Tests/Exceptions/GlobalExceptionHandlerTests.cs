using AutoVerdikt.WebApi.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace AutoVerdikt.WebApi.Tests.Exceptions;

public class GlobalExceptionHandlerTests
{
    private readonly Mock<IProblemDetailsService> _problemDetailsService = new();
    private readonly Mock<ILogger<GlobalExceptionHandler>> _logger = new();

    private GlobalExceptionHandler CreateHandler() =>
        new(_problemDetailsService.Object, _logger.Object);

    [Fact]
    public async Task TryHandleAsync_SetsResponseStatusTo500()
    {
        _problemDetailsService
            .Setup(s => s.TryWriteAsync(It.IsAny<ProblemDetailsContext>()))
            .ReturnsAsync(true);

        var httpContext = new DefaultHttpContext();
        var exception = new InvalidOperationException("boom");

        await CreateHandler().TryHandleAsync(httpContext, exception, CancellationToken.None);

        httpContext.Response.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);
    }

    [Fact]
    public async Task TryHandleAsync_ReturnsTrueWhenProblemDetailsWritten()
    {
        _problemDetailsService
            .Setup(s => s.TryWriteAsync(It.IsAny<ProblemDetailsContext>()))
            .ReturnsAsync(true);

        var httpContext = new DefaultHttpContext();

        var handled = await CreateHandler().TryHandleAsync(
            httpContext, new Exception("test"), CancellationToken.None);

        handled.ShouldBeTrue();
    }

    [Fact]
    public async Task TryHandleAsync_WritesProblemDetailsWithStatus500()
    {
        ProblemDetailsContext? capturedContext = null;
        _problemDetailsService
            .Setup(s => s.TryWriteAsync(It.IsAny<ProblemDetailsContext>()))
            .Callback<ProblemDetailsContext>(ctx => capturedContext = ctx)
            .ReturnsAsync(true);

        var exception = new InvalidOperationException("boom");
        var httpContext = new DefaultHttpContext();

        await CreateHandler().TryHandleAsync(httpContext, exception, CancellationToken.None);

        capturedContext.ShouldNotBeNull();
        capturedContext!.ProblemDetails.Status.ShouldBe(StatusCodes.Status500InternalServerError);
        capturedContext.Exception.ShouldBe(exception);
    }

    [Fact]
    public async Task TryHandleAsync_LogsExceptionAtErrorLevel()
    {
        _problemDetailsService
            .Setup(s => s.TryWriteAsync(It.IsAny<ProblemDetailsContext>()))
            .ReturnsAsync(true);

        var exception = new InvalidOperationException("boom");
        var httpContext = new DefaultHttpContext();

        await CreateHandler().TryHandleAsync(httpContext, exception, CancellationToken.None);

        _logger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
