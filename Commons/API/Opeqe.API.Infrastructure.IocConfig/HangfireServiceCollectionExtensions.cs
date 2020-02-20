using Hangfire;
using Hangfire.SqlServer;
using HSP.Infrastructure.Settings;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HSP.NGT.API.Infrastructure.IocConfig
{
    public static class HangfireServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomHangfire(this IServiceCollection services, SiteSettings siteSettings)
        {
            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(siteSettings.ConnectionStrings.SqlServer.HangfireConnection, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            return services;
        }
    }

}
