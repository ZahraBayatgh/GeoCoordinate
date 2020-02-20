using HSP.Infrastructure.Identity.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HSP.Infrastructure.IocConfig
{
    public static class ServicesRegistry
    {
        /// <summary>
        /// Creates and seeds the database.
        /// </summary>
        public static void InitializeDb(this IServiceProvider serviceProvider)
        {
            var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var identityDbInitialize = scope.ServiceProvider.GetService<IIdentityDbInitializer>();
                identityDbInitialize.Initialize();
                identityDbInitialize.SeedData();
            }
        }

    }
}