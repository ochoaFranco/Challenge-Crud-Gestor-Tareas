using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using TaskManagement.Domain.Exceptions;

namespace TaskManagement.Api
{
    public sealed class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var (status, title, detail) = exception switch
            {
                KeyNotFoundException =>
                    (StatusCodes.Status404NotFound, "Not Found", exception.Message),
                
                DuplicatedTaskTitleException =>
                    (StatusCodes.Status409Conflict, "Conflict", exception.Message),

                _ =>
                    (StatusCodes.Status500InternalServerError, "Internal Server Error", "An unexpected error occurred.")
            };

            await Results.Problem(
                statusCode: status,
                title: title,
                detail: detail,
                type:$"https://httpstatuses.com/{status}",
                instance: httpContext.Request.Path
                ).ExecuteAsync(httpContext);

            return true;
        }
    }
}