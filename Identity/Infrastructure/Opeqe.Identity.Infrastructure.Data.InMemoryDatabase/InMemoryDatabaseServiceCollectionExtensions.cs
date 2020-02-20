using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Opeqe.Identity.Infrastructure.Data.Context;
using Opeqe.Identity.Infrastructure.Settings;
using Opeqe.Infrastructure.Data.Context;
using System;

namespace Opeqe.Identity.Infrastructure.Data.InMemoryDatabase
{
    public static class InMemoryDatabaseServiceCollectionExtensions
    {
        public static IServiceCollection AddConfiguredInMemoryDbContext(this IServiceCollection services, IdentitySettings siteSettings)
        {
            services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<CustomIdentityDbContext>());
            services.AddEntityFrameworkInMemoryDatabase(); // It's added to access services from the dbcontext, remove it if you are using the normal `AddDbContext` and normal constructor dependency injection.
            services.AddDbContextPool<CustomIdentityDbContext, InMemoryDatabaseContext>(
                (serviceProvider, optionsBuilder) => optionsBuilder.UseConfiguredInMemoryDatabase(siteSettings, serviceProvider));
            return services;
        }

        public static void UseConfiguredInMemoryDatabase(
            this DbContextOptionsBuilder optionsBuilder, IdentitySettings siteSettings, IServiceProvider serviceProvider)
        {
            optionsBuilder.UseInMemoryDatabase(siteSettings.ConnectionStrings.LocalDb.InitialCatalog);
            optionsBuilder.UseInternalServiceProvider(serviceProvider); // It's added to access services from the dbcontext, remove it if you are using the normal `AddDbContext` and normal constructor dependency injection.
            optionsBuilder.ConfigureWarnings(warnings =>
            {
                // ...
            });
        }
    }
}