using Microsoft.Extensions.DependencyInjection;
using Opeqe.Identity.Infrastructure.Data.InMemoryDatabase;
using Opeqe.Identity.Infrastructure.Data.MSSQL;
using Opeqe.Identity.Infrastructure.Services.Contracts;
using Opeqe.Identity.Infrastructure.Settings;
using System;

namespace Opeqe.Identity.API.Infrastructure.IocConfig
{
    public static class DbContextOptionsExtensions
    {
        public static IServiceCollection AddConfiguredDbContext(
            this IServiceCollection serviceCollection, IdentitySettings siteSettings)
        {
            switch (siteSettings.ActiveDatabase)
            {
                case ActiveDatabase.InMemoryDatabase:
                    serviceCollection.AddConfiguredInMemoryDbContext(siteSettings);
                    break;

                case ActiveDatabase.SqlServer:
                    serviceCollection.AddConfiguredMsSqlDbContext(siteSettings);
                    break;


                default:
                    throw new NotSupportedException("Please set the ActiveDatabase in appsettings.json file.");
            }

            return serviceCollection;
        }

        /// <summary>
        /// Creates and seeds the database.
        /// </summary>
        public static void InitializeDb(this IServiceProvider serviceProvider)
        {
            var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var identityDbInitialize = scope.ServiceProvider.GetRequiredService<IIdentityDbInitializer>();
                identityDbInitialize.Initialize();
                identityDbInitialize.SeedData();
            }
        }
    }
}