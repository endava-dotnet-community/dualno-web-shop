using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using WebShop.Authentication;

namespace WebShop.Middleware
{
    /// <summary>
    /// Api key authentication middleware
    /// </summary>
    public class ApiKeyAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AuthOptions _authConstants;

        /// <summary>
        /// Constructor for <see cref=ApiKeyAuthMiddleware/> class
        /// </summary>
        /// <param name="next">Delegate to next step in pipeline</param>
        /// <param name="options">Authentication options object</param>
        public ApiKeyAuthMiddleware(RequestDelegate next, IOptions<AuthOptions> options)
        {
            _next = next;
            _authConstants = options.Value;
        }

        /// <summary>
        /// Invoke async method
        /// </summary>
        /// <param name="context">Http context object</param>
        public async Task InvokeAsync(HttpContext context)
        {
            StringValues extractedApiKey = "";
            if (!context.Request.Headers.TryGetValue(nameof(_authConstants.ApiKey), out extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API key missing!");
                return;
            }

            if (!_authConstants.ApiKey.Equals(extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid API key!");
                return;
            }

            await _next(context);
        }
    }
}
