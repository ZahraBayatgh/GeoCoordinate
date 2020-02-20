using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Opeqe.Common.EventBusRabbitMQ;
using Opeqe.Common.IntegrationEventLogEF;
using Opeqe.Common.IntegrationEventLogEF.Services;
using Opeqe.Identity.Infrastructure.Settings;
using RabbitMQ.Client;
using System;
using System.Data.Common;

namespace Opeqe.Identity.API.Infrastructure.IocConfig
{
    public static class CustomIntegrationServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomIntegrations(this IServiceCollection services, IdentitySettings siteSettings)
        {
            services.AddDbContext<IntegrationEventLogContext>(options =>
            {
                options.UseSqlServer(siteSettings.ConnectionStrings.SqlServer.DefaultConnection,
                                     sqlServerOptionsAction: sqlOptions =>
                                     {
                                         sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                                     });
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
                sp => (DbConnection c) => new IntegrationEventLogService(c));



            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();


                var factory = new ConnectionFactory()
                {
                    HostName = siteSettings.EventBusConnection,
                    DispatchConsumersAsync = true
                };

                if (!string.IsNullOrEmpty(siteSettings.EventBusUserName))
                {
                    factory.UserName = siteSettings.EventBusUserName;
                }

                if (!string.IsNullOrEmpty(siteSettings.EventBusPassword))
                {
                    factory.Password = siteSettings.EventBusPassword;
                }

                var retryCount = 5;
                if (siteSettings.EventBusRetryCount != 0)
                {
                    retryCount = siteSettings.EventBusRetryCount;
                }

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });

            return services;
        }
    }
    public static class IdentityServicesRegistry
    {
        /// <summary>
        /// Adds all of the ASP.NET Core Identity related services and configurations at once.
        /// </summary>
        public static void AddCustomIdentityServices(this IServiceCollection services, IdentitySettings siteSettings)
        {
            services.AddIdentityOptions(siteSettings);
            services.AddConfiguredDbContext(siteSettings);
            services.AddCustomServices();
            services.AddDynamicPermissions();
        }
    }
}