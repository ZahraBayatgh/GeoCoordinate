using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Opeqe.Common.EventBus;
using Opeqe.Common.EventBus.Abstractions;
using Opeqe.Common.EventBusRabbitMQ;
using Opeqe.Identity.Infrastructure.Settings;

namespace Opeqe.Identity.API.Infrastructure.IocConfig
{
    public static class EventBusServiceCollectionExtensions
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services, IdentitySettings siteSettings)
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