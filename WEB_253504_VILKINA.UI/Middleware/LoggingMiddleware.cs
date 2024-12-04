using Serilog;

namespace WEB_253504_VILKINA.UI.Middleware
{
    public class LoggingMiddleware
    {
        RequestDelegate _next;
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode < 200 || context.Response.StatusCode >= 300)
            {
                var url = context.Request.Path;

                Log.Information("---> request {Url} returns {StatusCode}", url, context.Response.StatusCode) ;
            }
        }
    }
}
