using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware;

/// <summary>
/// Middleware to handle exceptions globally in the application.
/// It intercepts exceptions, logs them, and formats the response to the client.
/// </summary>
public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env)
{
    /// <summary>
    /// Middleware invocation method to handle the HTTP context.
    /// Catches exceptions, logs them, and writes a formatted response.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            // Log the exception details
            logger.LogError(ex, ex.Message);
            // Set response content type and status code
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            // Create an ApiException instance based on the environment
            var response = env.IsDevelopment()
                ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                : new ApiException(context.Response.StatusCode, ex.Message, "Internal Server Error");
            // Configure JSON serialization options
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            // Serialize the response and write it to the HTTP response body
            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }
}

