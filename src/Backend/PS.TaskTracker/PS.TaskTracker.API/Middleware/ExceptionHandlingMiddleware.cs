using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PS.TaskTracker.Application.Exceptions;
using PS.TaskTracker.Domain.Exceptions;
using AppValidationException = PS.TaskTracker.Application.Exceptions.ValidationException;

namespace PS.TaskTracker.API.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleAsync(context, ex);
        }
    }

    private async Task HandleAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title, detail, errors) = exception switch
        {
            AppValidationException ve => (
                StatusCodes.Status400BadRequest,
                "One or more validation errors occurred.",
                "The request body is invalid.",
                (IReadOnlyDictionary<string, string[]>?)ve.Errors),

            DomainValidationException dve => (
                StatusCodes.Status400BadRequest,
                "Validation error.",
                dve.Message,
                null),

            DomainConflictException dce => (
                StatusCodes.Status409Conflict,
                "Status transition conflict.",
                dce.Message,
                null),

            NotFoundException nfe => (
                StatusCodes.Status404NotFound,
                "Resource not found.",
                nfe.Message,
                null),

            _ => (
                StatusCodes.Status500InternalServerError,
                "An unexpected error occurred.",
                "The server failed to handle the request.",
                null)
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception");
        }

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        if (errors is not null)
        {
            problem.Extensions["errors"] = errors;
        }

        var json = JsonSerializer.Serialize(problem, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
