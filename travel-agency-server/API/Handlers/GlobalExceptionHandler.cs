using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace API.Handlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(
            ILogger<GlobalExceptionHandler> logger  
        )
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var (statusCode, message) = GetExceptionDetails(exception);

            _logger.LogError(exception, exception.Message);

            // assign status code to the response that is sent back to the client
            httpContext.Response.StatusCode = (int) statusCode;
            // write the response as json
            await httpContext.Response.WriteAsJsonAsync(message, cancellationToken);

            return true; // means that the exception has been handled
        }

        private (HttpStatusCode statusCode, string message) GetExceptionDetails(Exception exception)
        {
            return exception switch
            {
                LoginFailedException => (HttpStatusCode.Unauthorized, exception.Message),
                UserAlreadyExistsException => (HttpStatusCode.Conflict, exception.Message),
                RegistrationFailedException => (HttpStatusCode.BadRequest, exception.Message),
                RefreshTokenException => (HttpStatusCode.Unauthorized, exception.Message),
                // default one
                _ => (HttpStatusCode.InternalServerError, $"An unexpected error occurred: {exception.Message}")
            };
        }

    }
}
