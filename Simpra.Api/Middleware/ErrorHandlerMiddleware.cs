using Serilog;
using Simpra.Service.Exceptions;
using Simpra.Service.Response;
using System.Net;
using System.Text.Json;

namespace Simpra.Api.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate next;
        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Log.Information("LogErrorHandlerMiddleware.Invoke");
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var statusCode = ex switch
                {
                    ClientSideException => 400,
                    NotFoundException => 404,
                    _ => 500
                };

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                var response = CustomResponse<NoContent>.Fail(statusCode, ex.Message);

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }

    }
}
