using Autofac;
using Autofac.Extensions.DependencyInjection;
using DNTCommon.Web.Core;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Opeqe.Common.EventBus.Abstractions;
using Opeqe.Identity.API.Infrastructure.AutofacModules;
using Opeqe.Identity.API.Infrastructure.IocConfig;
using Opeqe.Identity.API.IntegrationEvents;
using Opeqe.Identity.Infrastructure.Services.Contracts;
using Opeqe.Infrastructure.Idempotency;
using Opeqe.Infrastructure.Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Opeqe.Identity.API
{
    public class Startup
    {
        private readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<Opeqe.Identity.Infrastructure.Settings.IdentitySettings>(options => Configuration.Bind(options));

            //var siteSettings = services.GetSiteSettings();
            var siteSettings = services.BuildServiceProvider().GetService<Microsoft.Extensions.Options.IOptionsSnapshot<Opeqe.Identity.Infrastructure.Settings.IdentitySettings>>().Value;
            services.AddSingleton(sp => siteSettings);

            services.AddCustomAuthentication(siteSettings);
            services.AddCustomIntegrations(siteSettings);
            services.AddTransient<IIdentityIntegrationEventService, IdentityIntegrationEventService>();
            services.AddEventBus(siteSettings);
            services.AddDNTCommonWeb();
            services.AddScoped<IRequestManager, RequestManager>();
            services.AddAutoMapper();
            services.AddCors(options =>
           {
               options.AddPolicy("CorsPolicy",
                   builder => builder
                   //.WithOrigins(Configuration["site"])
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .SetIsOriginAllowed((host) => true)
                   .AllowCredentials());
           });
            services.AddHttpClient();

            // Adds all of the ASP.NET Core Identity related services and configurations at once.
            services.AddCustomIdentityServices(siteSettings);

            services.AddMvc(options => options.UseYeKeModelBinder());

            services.AddHealthChecks(Configuration);

            //configure autofac
            var container = new ContainerBuilder();
            container.Populate(services);
            container.RegisterModule(new ApplicationModule());
            return new AutofacServiceProvider(container.Build());
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthorization();
            app.UseMiddleware<SerilogRequestLogger>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapAreaControllerRoute(
                    name: "areas",
                    areaName: "areas",
                    pattern: "{area:exists}/{controller=Account}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    "default", "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self")
                });
            });


            app.ConfigureEventBus();
        }
    }

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigureEventBus(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            // ToDo : Subscribe to events

            return app;
        }
    }
    public static class HealthChecksServiceCollectionExtensions
    {
        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var hcBuilder = services.AddHealthChecks();

            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

            hcBuilder
                .AddSqlServer(
                    configuration["ConnectionStrings:SqlServer:DefaultConnection"],
                    name: $"{Program.AppName}DB-check",
                    tags: new string[] { $"{Program.AppName}db" });


            hcBuilder
                .AddRabbitMQ(
                    $"amqp://{configuration["EventBusConnection"]}",
                    name: $"{Program.AppName}-rabbitmqbus-check",
                    tags: new string[] { $"{Program.AppName}rabbitmqbus" });

            return services;
        }
    }



}
