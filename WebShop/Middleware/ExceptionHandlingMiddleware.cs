using FluentValidation;
using Services.Exceptions;

namespace WebShop.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // request logic (/)
            try
            {
                // next
                await _next.Invoke(context);
            }
            catch (NotAuthorizedException ex)
            {
                await WriteResponseAsync(
                    context, 403, ex.Message);
            }
            catch (ValidationException ex)
            {
                await WriteResponseAsync(
                    context, 400, ex.Message);
            }
            catch (ResourceNotFoundException ex)
            {
                await WriteResponseAsync(
                    context, 204, ex.Message);
            }
            catch (Exception ex)
            {
                // response logic
                await WriteResponseAsync(
                    context, 500, ex.Message);
            }
        }

        private static async Task WriteResponseAsync(
            HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(message);
        }
    }
}