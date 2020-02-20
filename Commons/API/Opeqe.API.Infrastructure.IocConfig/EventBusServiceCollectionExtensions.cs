using Autofac;
using HSP.Common.EventBus;
using HSP.Common.EventBus.Abstractions;
using HSP.Common.EventBusRabbitMQ;
using HSP.Infrastructure.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HSP.NGT.API.Infrastructure.IocConfig
{
    public static class EventBusServiceCollectionExtensions
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services, SiteSettings siteSettings)
        {
            var subscriptionClientName = siteSettings.SubscriptionClientName;

            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                var retryCount = 5;
                if (siteSettings.EventBusRetryCount != 0)
                {
                    retryCount = siteSettings.EventBusRetryCount;
                }

                return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            return services;
        }
    }

}
