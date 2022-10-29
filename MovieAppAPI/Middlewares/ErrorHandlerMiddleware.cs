using System.Net;
using System.Text.Json;
using MovieAppAPI.Exceptions;

namespace MovieAppAPI.Middlewares;

public class ErrorHandlerMiddleware {
    private readonly RequestDelegate _next;
    private readonly ILogger<object> _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<object> logger) {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context) {
        try {
            await _next(context);
        }
        catch (Exception error) {
            var response = context.Response;
            response.ContentType = "application/json";

            switch (error) {
                case RecordNotFoundException e:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case AlreadyExistsException e:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    break;
                default:
                    _logger.LogError(error, error.Message);
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            
            var result = JsonSerializer.Serialize(new { message = error.Message });
            await response.WriteAsync(result);
        }
    }
}