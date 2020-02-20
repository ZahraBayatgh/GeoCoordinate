using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Opeqe.Identity.API.Infrastructure.IocConfig
{
    public static class HealthChecksServiceCollectionExtensions
    {
        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var hcBuilder = services.AddHealthChecks();

            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

            hcBuilder
                .AddSqlServer(
                    configuration["ConnectionStrings:SqlServer:DefaultConnection"],
                    name: "IdentityDB-check",
                    tags: new string[] { "orderingdb" });

            hcBuilder
                .AddRabbitMQ(
                    $"amqp://{configuration["EventBusConnection"]}",
                    name: "indentity-rabbitmqbus-check",
                    tags: new string[] { "rabbitmqbus" });

            return services;
        }
    }
}