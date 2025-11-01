using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Play.Common
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var errorResponse = new
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Result = (object)null
                };

                var json = System.Text.Json.JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(json);
            }
        }
    }

}
