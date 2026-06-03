using System.Net;
using AutoVerdikt.Application.Errors;
using AutoVerdikt.Application.Users.Errors;
using Shouldly;

namespace AutoVerdikt.Application.Tests.Users.Errors;

public class UserAlreadyRegisteredErrorTests
{
    private readonly UserAlreadyRegisteredError _error = new();

    [Fact]
    public void ErrorCode_IsUSR001() =>
        _error.ErrorCode.ShouldBe("USR-001");

    [Fact]
    public void HttpStatusCode_IsConflict() =>
        _error.HttpStatusCode.ShouldBe(HttpStatusCode.Conflict);

    [Fact]
    public void Message_IsDescriptive() =>
        _error.Message.ShouldBe("User is already registered.");

    [Fact]
    public void IsDomainError() =>
        _error.ShouldBeAssignableTo<DomainError>();
}
