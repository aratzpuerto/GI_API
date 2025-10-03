using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace GI_API.Middlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger) {
            _next = next;
            _logger = logger;
        }

        
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unhandled exception occurred while processing request");

                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                ProblemDetails problems = new()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = "Server error",
                    Title = "Server error",
                    Detail = "An internal server error has occured."                    
                };

                string json = JsonSerializer.Serialize(problems);

                context.Response.ContentType = "application/json";

                _logger.LogError(JsonSerializer.Serialize(problems));
                _logger.LogError(e, e.Message);


                await context.Response.WriteAsync(json);


            }

            
        }

    }
}
