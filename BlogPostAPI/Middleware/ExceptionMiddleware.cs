﻿using Application.Exceptions;
using Application.Models;
using Newtonsoft.Json;
using System.Net;

namespace BlogPostAPI.Middleware;
public class ExceptionMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex, CancellationToken cancellationToken = default)
    {
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
        CustomProblemDetails problem = new();
        switch (ex)
        {
            case InvalidApiKeyException InvalidApiKeyException:
                statusCode = HttpStatusCode.Unauthorized;
                problem = new CustomProblemDetails
                {
                    Title = InvalidApiKeyException.Message,
                    Status = (int)statusCode,
                    Detail = InvalidApiKeyException.InnerException?.Message,
                    Type = nameof(InvalidApiKeyException)                    
                };
                break;
            case NotFoundException NotFound:
                statusCode = HttpStatusCode.NotFound;
                problem = new CustomProblemDetails
                {
                    Title = NotFound.Message,
                    Status = (int)statusCode,
                    Type = nameof(NotFoundException),
                    Detail = NotFound.InnerException?.Message,
                };
                break;
            case UserAlreadyExistsException UserAlreadyExistsException:
                statusCode = HttpStatusCode.Conflict;
                problem = new CustomProblemDetails
                {
                    Title = UserAlreadyExistsException.Message,
                    Status = (int)statusCode,
                    Type = nameof(UserAlreadyExistsException),
                    Detail = UserAlreadyExistsException.InnerException?.Message,
                };
                break;
            case ErrorRegisteringUserException ErrorRegisteringUserException:
                statusCode = HttpStatusCode.BadRequest;
                problem = new CustomProblemDetails
                {
                    Title = ErrorRegisteringUserException.Message,
                    Status = (int)statusCode,
                    Type = nameof(ErrorRegisteringUserException),
                    Detail = ErrorRegisteringUserException.InnerException?.Message,
                };
                break;
            case InvalidUserPasswordException InvalidUserPasswordException:
                statusCode = HttpStatusCode.BadRequest;
                problem = new CustomProblemDetails
                {
                    Title = InvalidUserPasswordException.Message,
                    Status = (int)statusCode,
                    Type = nameof(InvalidUserPasswordException),
                    Detail = InvalidUserPasswordException.InnerException?.Message,
                };
                break;
            default:
                statusCode = HttpStatusCode.InternalServerError;
                problem = new CustomProblemDetails
                {
                    Title = ex.Message,
                    Status = (int)statusCode,
                    Type = nameof(HttpStatusCode.InternalServerError),
                    Detail = ex.StackTrace,
                };
                break;
        }

        httpContext.Response.StatusCode = (int)statusCode;
        var logMessage = JsonConvert.SerializeObject(problem);
        _logger.LogError(logMessage);
        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsync(logMessage, cancellationToken);
    }
}