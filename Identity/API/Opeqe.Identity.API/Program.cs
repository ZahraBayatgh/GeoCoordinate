using Grpc.Core;
using MagicOnion.Server;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Opeqe.Identity.API.Infrastructure.IocConfig;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;

namespace Opeqe.Identity.API
{
    public class Program
    {
        public static readonly string AppName = "Identity";

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
                StartgRPC();
                host.Services.InitializeDb();
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

        private static void StartgRPC()
        {
            GrpcEnvironment.SetLogger(new Grpc.Core.Logging.ConsoleLogger());

            // setup MagicOnion and option.
            var service = MagicOnionEngine.BuildServerServiceDefinition(isReturnExceptionStackTraceInErrorDetail: true);

            var server = new Server
            {
                Services = { service },
                Ports = { new ServerPort("localhost", 12345, ServerCredentials.Insecure) }
            };

            // launch gRPC Server.
            server.Start();
        }

    }
}
