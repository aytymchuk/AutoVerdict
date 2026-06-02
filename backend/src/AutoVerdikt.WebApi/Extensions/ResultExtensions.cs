using AutoVerdikt.Application.Errors;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace AutoVerdikt.WebApi.Extensions;

internal static class ResultExtensions
{
    internal static IResult ToProblemResult(this IResultBase result)
    {
        var domainError = result.Errors.OfType<DomainError>().FirstOrDefault();

        if (domainError is not null)
        {
            var statusCode = (int)domainError.HttpStatusCode;
            return Results.Problem(new ProblemDetails
            {
                Type = $"https://autoverdikt.com/errors/{domainError.ErrorCode.ToLowerInvariant()}",
                Title = domainError.Message,
                Status = statusCode,
                Extensions = { ["errorCode"] = domainError.ErrorCode }
            });
        }

        // Fallback: non-DomainError in a Result (should not occur under normal operation)
        return Results.Problem(
            title: "An unexpected error occurred",
            statusCode: StatusCodes.Status500InternalServerError);
    }
}
