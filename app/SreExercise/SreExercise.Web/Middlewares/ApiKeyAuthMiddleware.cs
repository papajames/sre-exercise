using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace SreExercise.Web.Middlewares
{
    public class ApiKeyAuthMiddleware
    {
        public const string HeaderName = "X-ApiKey";

        private readonly string _apiKey;
        private readonly RequestDelegate _next;

        public ApiKeyAuthMiddleware(IConfiguration configuration, RequestDelegate next)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if ((_apiKey = configuration.GetValue<string>("ApiKey")) == null)
            {
                throw new ArgumentException(
                    "Cannot find the value of 'ApiKey' in app settings.",
                    nameof(configuration)
                );
            }

            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestPath = context.Request.Path;
            var apiKeys = context.Request.Headers[HeaderName];
            if(requestPath.Value.StartsWith("/api") && !apiKeys.Contains(_apiKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            await _next(context);
        }
    }
}