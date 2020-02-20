using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Opeqe.Infrastructure.Extensions
{
    public static class CacheManagerExtentions
    {
        /// <summary>
        /// Sets `no-cache`, `must-revalidate`, `no-store` headers for the current `Response`.
        /// </summary>
        public static void DisableBrowserCache(this HttpContext httpContext)
        {
            httpContext.Response.Headers["Cache-Control"] = new StringValues(new string[4]
            {
                "no-cache",
                "max-age=0",
                "must-revalidate",
                "no-store"
            });
            httpContext.Response.Headers["Expires"] = "-1";
            httpContext.Response.Headers["Pragma"] = "no-cache";
        }
    }

}
