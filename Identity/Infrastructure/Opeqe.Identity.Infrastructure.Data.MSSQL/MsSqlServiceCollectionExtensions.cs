using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Opeqe.Identity.Infrastructure.Data.Context;
using Opeqe.Identity.Infrastructure.Settings;
using Opeqe.Infrastructure.Data.Context;
using System;

namespace Opeqe.Identity.Infrastructure.Data.MSSQL
{
    public static class MsSqlServiceCollectionExtensions
    {
        public static IServiceCollection AddConfiguredMsSqlDbContext(this IServiceCollection services, IdentitySettings IdentitySettings)
        {
            services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<CustomIdentityDbContext>());
            services.AddEntityFrameworkSqlServer(); // It's added to access services from the dbcontext, remove it if you are using the normal `AddDbContext` and normal constructor dependency injection.
            services.AddDbContextPool<CustomIdentityDbContext, MsSqlDbContext>(
                (serviceProvider, optionsBuilder) => optionsBuilder.UseConfiguredMsSql(IdentitySettings, serviceProvider));
            return services;
        }

        public static void UseConfiguredMsSql(
            this DbContextOptionsBuilder optionsBuilder, IdentitySettings IdentitySettings, IServiceProvider serviceProvider)
        {
            var connectionString = IdentitySettings.GetMsSqlDbConnectionString();
            optionsBuilder.UseSqlServer(
                        connectionString,
                        sqlServerOptionsBuilder =>
                        {
                            sqlServerOptionsBuilder.CommandTimeout((int)TimeSpan.FromMinutes(3).TotalSeconds);
                            sqlServerOptionsBuilder.EnableRetryOnFailure();
                            sqlServerOptionsBuilder.MigrationsAssembly(typeof(MsSqlServiceCollectionExtensions).Assembly.FullName);
                        });
            optionsBuilder.UseInternalServiceProvider(serviceProvider); // It's added to access services from the dbcontext, remove it if you are using the normal `AddDbContext` and normal constructor dependency injection.
            optionsBuilder.ConfigureWarnings(warnings =>
            {
                // ...
            });
        }

        public static string GetMsSqlDbConnectionString(this IdentitySettings IdentitySettingsValue)
        {
            if (IdentitySettingsValue == null)
            {
                throw new ArgumentNullException(nameof(IdentitySettingsValue));
            }

            switch (IdentitySettingsValue.ActiveDatabase)
            {
                //case ActiveDatabase.LocalDb:
                //    var attachDbFilename = IdentitySettingsValue.ConnectionStrings.LocalDb.AttachDbFilename;
                //    var attachDbFilenamePath = Path.Combine(ServerInfo.GetAppDataFolderPath(), attachDbFilename);
                //    var localDbInitialCatalog = IdentitySettingsValue.ConnectionStrings.LocalDb.InitialCatalog;
                //    return $@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog={localDbInitialCatalog};AttachDbFilename={attachDbFilenamePath};Integrated Security=True;MultipleActiveResultSets=True;";

                case ActiveDatabase.SqlServer:
                    return IdentitySettingsValue.ConnectionStrings.SqlServer.DefaultConnection;

                default:
                    throw new NotSupportedException("Please set the ActiveDatabase in appsettings.json file to `LocalDb` or `SqlServer`.");
            }
        }
    }
}
