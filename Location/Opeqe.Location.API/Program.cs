using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;

namespace Location.API
{
    public class Program
    {
        public static readonly string AppName = "Location";

        public static int Main(string[] args)
        {
            var configuration = GetConfiguration();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            // If we have problems getting the log to write to SQL Server, we can use
            Serilog.Debugging.SelfLog.Enable(msg =>
            {
                Debug.Print(msg);
                Debugger.Break();
            });

            try
            {
                Log.Information("Configuring web host ({ApplicationContext})...", AppName);
                var host = BuildWebHost(configuration, args);


                Log.Information("Starting web host ({ApplicationContext})...", AppName);
                host.Run();


                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", AppName);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }

        private static IWebHost BuildWebHost(IConfiguration configuration, string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .CaptureStartupErrors(false)
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(configuration)
                .UseSerilog()
                .Build();
        }
    }
}
