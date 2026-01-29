using Microsoft.AspNetCore.Diagnostics;

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
                (404, "Not Found", exception.Message),
                _ =>
                (500, "Internal Server Error", "An unexpected error occurred.")
            };

            await Results.Problem(
                statusCode: status,
                title: title,
                detail: detail,
                instance: httpContext.Request.Path
                ).ExecuteAsync(httpContext);

            return true;
        }
    }
}