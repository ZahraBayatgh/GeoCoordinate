using HSP.Identity.Infrastructure.Entities;
using HSP.Identity.Infrastructure.Settings;
using HSP.Infrastructure.Toolkits.GuardToolkit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;

namespace HSP.Identity.Infrastructure.Services.Logger
{
    public class DbLogger : ILogger
    {
        private readonly string _loggerName;
        private readonly IServiceProvider _serviceProvider;
        private readonly DbLoggerProvider _loggerProvider;
        private readonly IdentitySettings _siteSettings;
        private readonly LogLevel _minLevel;

        public DbLogger(string loggerName,
                        DbLoggerProvider loggerProvider,
                        IServiceProvider serviceProvider,
                        IdentitySettings siteSettings
            )
        {
            _loggerName = loggerName ?? throw new ArgumentNullException(nameof(loggerName));
            _minLevel = _siteSettings.Logging.LogLevel.Default;
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _loggerProvider = loggerProvider ?? throw new ArgumentNullException(nameof(loggerProvider));
            _siteSettings = siteSettings ?? throw new ArgumentNullException(nameof(siteSettings));
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new NoopDisposable();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _minLevel;
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var message = formatter(state, exception);

            if (exception != null)
            {
                message = $"{message}{Environment.NewLine}{exception}";
            }

            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            var httpContextAccessor = _serviceProvider.GetService<IHttpContextAccessor>();
            var appLogItem = new AppLogItem
            {
                Url = httpContextAccessor?.HttpContext != null ? httpContextAccessor.HttpContext.Request.Path.ToString() : string.Empty,
                EventId = eventId.Id,
                LogLevel = logLevel.ToString(),
                Logger = _loggerName,
                Message = message
            };
            setStateJson(state, appLogItem);
            _loggerProvider.AddLogItem(appLogItem);
        }

        private static void setStateJson<TState>(TState state, AppLogItem appLogItem)
        {
            try
            {
                appLogItem.StateJson = JsonSerializer.Serialize(
                    state,
                    new JsonSerializerOptions
                    {
                        IgnoreNullValues = true,
                        WriteIndented = true
                    });
            }
            catch
            {
                // don't throw exceptions from logger
            }
        }

        private class NoopDisposable : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}