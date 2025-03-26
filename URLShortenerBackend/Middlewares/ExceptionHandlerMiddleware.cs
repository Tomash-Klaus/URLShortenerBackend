using System.Net;
using System.Text.Json;
using URLShortenerBackend.DTOs;

namespace URLShortenerBackend.Middlewares
{
    public class ExceptionHandlerMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); 
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            

            var errorResponse = new ErrorResponse { Message = exception.Message };

            switch (exception)
            {
                case BadHttpRequestException exception1:
                    errorResponse.Code = exception1.StatusCode;
                    break;
                default:
                    errorResponse.Code = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = errorResponse.Code;

            return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, options));
        }
    }
}
