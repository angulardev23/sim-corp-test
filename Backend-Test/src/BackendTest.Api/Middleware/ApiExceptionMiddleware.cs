using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using BackendTest.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BackendTest.Api.Middleware;

public sealed class ApiExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiExceptionMiddleware> _logger;

    public ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
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
        catch (Exception exception)
        {
            if (context.Response.HasStarted)
            {
                throw;
            }

            await WriteProblemAsync(context, exception);
        }
    }

    private async Task WriteProblemAsync(HttpContext context, Exception exception)
    {
        var (status, title, detail) = exception switch
        {
            ResourceNotFoundException => (HttpStatusCode.NotFound, "Resource not found", exception.Message),
            RequestConflictException => (HttpStatusCode.Conflict, "Request conflict", exception.Message),
            ArgumentException => (HttpStatusCode.BadRequest, "Invalid request", exception.Message),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred", "The request could not be completed.")
        };

        if (status == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "An unhandled exception occurred while processing {Method} {Path}.",
                context.Request.Method, context.Request.Path);
        }

        context.Response.Clear();
        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Status = (int)status,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        await JsonSerializer.SerializeAsync(context.Response.Body, problem, cancellationToken: context.RequestAborted);
    }
}
