using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Opeqe.Identity.Infrastructure.Data.Context;
using Opeqe.Identity.Infrastructure.Settings;
using System;
using System.IO;

namespace Opeqe.Identity.Infrastructure.Data.MSSQL
{
    public class MsSqlContextFactory : IDesignTimeDbContextFactory<MsSqlDbContext>
    {
        public MsSqlDbContext CreateDbContext(string[] args)
        {
            var services = new ServiceCollection();
            services.AddOptions();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ILoggerFactory, LoggerFactory>();

            var basePath = Directory.GetCurrentDirectory();
            Console.WriteLine($"Using `{basePath}` as the ContentRootPath");
            var configuration = new ConfigurationBuilder()
                                .SetBasePath(basePath)
                                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                .Build();
            services.AddSingleton<IConfigurationRoot>(provider => configuration);
            services.Configure<IdentitySettings>(options => configuration.Bind(options));

            var IdentitySettings = services.BuildServiceProvider().GetRequiredService<IdentitySettings>();
            IdentitySettings.ActiveDatabase = ActiveDatabase.SqlServer;

            services.AddEntityFrameworkSqlServer(); // It's added to access services from the dbcontext, remove it if you are using the normal `AddDbContext` and normal constructor dependency injection.
            var optionsBuilder = new DbContextOptionsBuilder<CustomIdentityDbContext>();
            optionsBuilder.UseConfiguredMsSql(IdentitySettings, services.BuildServiceProvider());

            return new MsSqlDbContext(optionsBuilder.Options);
        }
    }
}
