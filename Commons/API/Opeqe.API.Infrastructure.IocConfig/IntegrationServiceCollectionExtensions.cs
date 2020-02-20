using HSP.Common.EventBusRabbitMQ;
using HSP.Common.IntegrationEventLogEF;
using HSP.Common.IntegrationEventLogEF.Services;
using HSP.NGT.Application.IntegrationEvents;
using HSP.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Data.Common;

namespace HSP.NGT.API.Infrastructure.IocConfig
{
    public static class AddCustomIntegrationServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomIntegrations(this IServiceCollection services, SiteSettings siteSettings)
        {
            services.AddDbContext<IntegrationEventLogContext>(options =>
            {
                options.UseSqlServer(siteSettings.ConnectionStrings.SqlServer.MarketDbContextConnection,
                                     sqlServerOptionsAction: sqlOptions =>
                                     {
                                         sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                                     });
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
                sp => (DbConnection c) => new IntegrationEventLogService(c));

            services.AddTransient<IApplicationIntegrationEventService, ApplicationIntegrationEventService>();

            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();


                var factory = new ConnectionFactory()
                {
                    HostName = siteSettings.EventBusConnection,
                    //DispatchConsumersAsync = true
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
}
