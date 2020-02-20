using Opeqe.Infrastructure.Serilog;

namespace Microsoft.AspNetCore.Builder
{
    public static class SerilogRequestLoggerMiddlewareExtensions
    {
        public static IApplicationBuilder UseSerilogRequestLogger(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SerilogRequestLogger>();
        }
    }
}
