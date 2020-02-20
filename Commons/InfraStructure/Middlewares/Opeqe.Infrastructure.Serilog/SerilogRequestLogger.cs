using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opeqe.Infrastructure.Serilog
{
    public class SerilogRequestLogger
    {
        private readonly RequestDelegate _next;

        public SerilogRequestLogger(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var RequestId = httpContext.TraceIdentifier;
            httpContext.Response.Headers["RequestId"] = RequestId.ToString();

            // Getting the request body is a little tricky because it's a stream
            // So, we need to read the stream and then rewind it back to the beginning
            string requestBody = "";
            httpContext.Request.EnableBuffering();
            Stream body = httpContext.Request.Body;
            byte[] buffer = new byte[Convert.ToInt32(httpContext.Request.ContentLength)];
            await httpContext.Request.Body.ReadAsync(buffer, 0, buffer.Length);
            requestBody = Encoding.UTF8.GetString(buffer);
            body.Seek(0, SeekOrigin.Begin);
            httpContext.Request.Body = body;

            LogForContext(httpContext)
                .ForContext("RequestBody", requestBody)
                .Debug("Request information {RequestMethod} {RequestPath} information with RequestId:{@RequestId} in {Elapsed:0.0000} ms"
                , httpContext.Request.Method, httpContext.Request.Path, RequestId, stopwatch.Elapsed.TotalMilliseconds);

            // The reponse body is also a stream so we need to:
            // - hold a reference to the original response body stream
            // - re-point the response body to a new memory stream
            // - read the response body after the request is handled into our memory stream
            // - copy the response in the memory stream out to the original response stream
            using (var responseBodyMemoryStream = new MemoryStream())
            {
                var originalResponseBodyReference = httpContext.Response.Body;
                httpContext.Response.Body = responseBodyMemoryStream;

                try
                {
                    await _next(httpContext);

                    stopwatch.Stop();
                }
                catch (Exception exception)
                {
                    stopwatch.Stop();

                    LogForContext(httpContext)
                        .ForContext("Exception", exception, destructureObjects: true)
                        .Error(exception, "Error information {RequestMethod} {RequestPath} responded {StatusCode} with RequestId:{@RequestId} in {Elapsed:0.0000} ms",
                        httpContext.Request.Method, httpContext.Request.Path, 500, RequestId, stopwatch.Elapsed.TotalMilliseconds);

                    if (!httpContext.Request.Headers.ContainsKey("UseDeveloperExceptionPage"))
                    {
                        var result = JsonConvert.SerializeObject(new { error = "Sorry, an unexpected error has occurred", RequestId = RequestId });
                        httpContext.Response.ContentType = "application/json";
                        httpContext.Response.StatusCode = 500;
                        await httpContext.Response.WriteAsync(result);
                    }
                    else
                    {
                        httpContext.Response.Body = originalResponseBodyReference;
                        throw;
                    }
                }

                httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
                string responseBody = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
                httpContext.Response.Body.Seek(0, SeekOrigin.Begin);

                ILogger logger = LogForContext(httpContext)
                    .ForContext<SerilogRequestLogger>()
                    .ForContext("RequestBody", requestBody)
                    .ForContext("ResponseBody", responseBody, destructureObjects: true);

                if (httpContext.Request.HasFormContentType)
                {
                    logger = logger.ForContext("RequestForm", httpContext.Request.Form.ToDictionary(v => v.Key, v => v.Value.ToString()));
                }

                logger.Debug("Response information {RequestMethod} {RequestPath} responded {StatusCode} with RequestId Id:{@RequestId} in {Elapsed:0.0000} ms"
                    , httpContext.Request.Method, httpContext.Request.Path, httpContext.Response?.StatusCode, RequestId, stopwatch.Elapsed.TotalMilliseconds);

                await responseBodyMemoryStream.CopyToAsync(originalResponseBodyReference);
            }
        }

        private ILogger LogForContext(HttpContext httpContext)
        {
            return Log
                .ForContext<SerilogRequestLogger>()
                .ForContext("UserName", httpContext.User.Identity.Name)
                .ForContext("RequestHeaders", httpContext.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
                .ForContext("RequestQueryString", httpContext.Request.QueryString)
                .ForContext("RequestHost", httpContext.Request.Host)
                .ForContext("RequestProtocol", httpContext.Request.Protocol)
                .ForContext("Claims", httpContext.User.Claims
                .GroupBy(x => x.Type).Select(x => new { Key = x.Key, Value = x.First().Value })
                .ToDictionary(h => h.Key, h => h.Value), destructureObjects: true);
        }
    }
}
