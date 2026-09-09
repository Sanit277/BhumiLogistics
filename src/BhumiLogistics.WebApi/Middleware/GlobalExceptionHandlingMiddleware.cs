using System.Net;
using BhumiLogistics.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BhumiLogistics.WebApi.Middleware;

/// <summary>
/// Centralized exception-to-HTTP-response translation, keeping controllers
/// and handlers free of try/catch boilerplate.
/// </summary>
public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
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
            _logger.LogError(ex, "Unhandled exception occurred while processing {Path}", context.Request.Path);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title) = exception switch
        {
            ValidationException => (HttpStatusCode.BadRequest, "One or more validation errors occurred."),
            PlotAlreadyLeasedException => (HttpStatusCode.Conflict, exception.Message),
            DomainException => (HttpStatusCode.BadRequest, exception.Message),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
        };

        var problemDetails = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Extensions =
            {
                ["errors"] = exception is ValidationException vex
                    ? vex.Errors.GroupBy(e => e.PropertyName)
                                 .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
                    : null
            }
        };

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;
        return context.Response.WriteAsJsonAsync(problemDetails);
    }
}
