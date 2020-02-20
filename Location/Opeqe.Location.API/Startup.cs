using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Location.API.Application.Behaviors.IntegrationEvents.EventHandling;
using Location.API.Application.Behaviors.IntegrationEvents.Events;
using Location.API.Infrastructure.AutofacModules;
using Location.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Opeqe.Common.EventBus;
using Opeqe.Common.EventBus.Abstractions;
using Opeqe.Common.EventBusRabbitMQ;
using Opeqe.Common.IntegrationEventLogEF.Services;
using RabbitMQ.Client;
using System;
using System.Data.Common;
using System.Text;

namespace Location.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication()
              .AddCookie()
              .AddJwtBearer(cfg =>
              {
                  cfg.TokenValidationParameters = new TokenValidationParameters()
                  {
                      ValidIssuer = Configuration["Tokens:Issuer"],
                      ValidAudience = Configuration["Tokens:Audience"],
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
                  };

              });
            services.AddDbContext<LocationDbContext>(options =>
                   options.UseSqlServer(Configuration["ConnectionString"]));

            services.AddCustomIntegrations(Configuration);
            services.AddEventBus(Configuration);
            services.AddControllers().AddFluentValidation();

            //configure autofac

            var container = new ContainerBuilder();
            container.Populate(services);

            container.RegisterModule(new MediatorModule());

            return new AutofacServiceProvider(container.Build());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.ConfigureEventBus();
        }
    }

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigureEventBus(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<UserCreatedIntegrationEvent, UserCreatedIntegrationEventHandler>();

            return app;
        }
    }
    public static class EventBusServiceCollectionExtensions
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            var subscriptionClientName = configuration["SubscriptionClientName"];


            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                var retryCount = 5;
                if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
                {
                    retryCount = int.Parse(configuration["EventBusRetryCount"]);
                }

                return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            return services;

        }
    }
    static class CustomIntegrationsExtensionsMethods
    {
        public static IServiceCollection AddCustomIntegrations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
                sp => (DbConnection c) => new IntegrationEventLogService(c));

            //services.AddTransient<IOrderingIntegrationEventService, OrderingIntegrationEventService>();

            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();


                var factory = new ConnectionFactory()
                {
                    HostName = configuration["EventBusConnection"],
                    DispatchConsumersAsync = true
                };

                if (!string.IsNullOrEmpty(configuration["EventBusUserName"]))
                {
                    factory.UserName = configuration["EventBusUserName"];
                }

                if (!string.IsNullOrEmpty(configuration["EventBusPassword"]))
                {
                    factory.Password = configuration["EventBusPassword"];
                }

                var retryCount = 5;
                if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
                {
                    retryCount = int.Parse(configuration["EventBusRetryCount"]);
                }

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });

            return services;
        }
    }

}
