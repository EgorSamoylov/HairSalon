using System.Diagnostics;

namespace Api.Middleware
{
    public class PerformanceMiddleware(
    RequestDelegate next,
    ILogger<PerformanceMiddleware> logger,
    TimeSpan? threshold = null)
    {
        private readonly TimeSpan _threshold = threshold ?? TimeSpan.FromMilliseconds(500);

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                await next(context);
            }
            finally
            {
                sw.Stop();
                var elapsed = sw.Elapsed;

                if (elapsed > _threshold)
                    logger.LogWarning(
                        "Long running request: {Method} {Path} took {ElapsedMilliseconds}ms with status {StatusCode}",
                        context.Request.Method,
                        context.Request.Path,
                        elapsed.TotalMilliseconds,
                        context.Response.StatusCode);
            }
        }
    }
}
