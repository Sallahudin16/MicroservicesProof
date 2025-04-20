using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SharedKernel.Exceptions.Handler
{
    public class CustomExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<CustomExceptionHandler> _logger;

        public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(
                "Error Message: {exceptionMessage}, Time of occurence {time}",
                 exception.Message, DateTime.UtcNow
            );


            (string Detail, string Title, int StatusCode) = exception switch
            {
                InternalServerException =>
                (
                    exception.Message,
                    exception.GetType().Name,
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError
                ),
                ValidationException =>
                (
                    exception.Message,
                    exception.GetType().Name,
                    context.Response.StatusCode = StatusCodes.Status400BadRequest
                ),
                BadRequestException =>
                (
                    exception.Message,
                    exception.GetType().Name,
                    context.Response.StatusCode = StatusCodes.Status400BadRequest
                ),
                NotFoundException =>
                (
                    exception.Message,
                    exception.GetType().Name,
                    context.Response.StatusCode = StatusCodes.Status404NotFound
                ),
                _ => ("Something Went Wrong", exception.GetType().Name, context.Response.StatusCode = StatusCodes.Status500InternalServerError)
            };

            ProblemDetails problemDetailes = new()
            {
                Title = Title,
                Detail = Detail,
                Status = StatusCode,
            };

            problemDetailes.Extensions.Add("traceId", context.TraceIdentifier);
            if(exception is ValidationException validationException)
            {
                problemDetailes.Extensions.Add("ValidationErrors", validationException.Errors);
            }

            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsJsonAsync(problemDetailes, cancellationToken: cancellationToken);
            return true;
        }
    }
}
