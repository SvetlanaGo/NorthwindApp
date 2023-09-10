using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;

namespace NorthwindApiApp
{
    /// <summary>
    /// Program class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// A program entry point.
        /// </summary>
        /// <param name="args">Program arguments.</param>
        public static void Main(string[] args)
        {
            const string connectionStringName = "NORTHWIND_BLOGGING";
            const string connectionStringPrefix = "SQLCONNSTR_";
            const string connectionString = "data source = (localdb)\\MSSQLLocalDB; Integrated Security = True; Initial Catalog = NorthwindBlogging;";
            if (Environment.GetEnvironmentVariable($"{connectionStringPrefix}{connectionStringName}") is null)
            {
                Environment.SetEnvironmentVariable($"{connectionStringPrefix}{connectionStringName}", connectionString);
            }

            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

            try
            {
                logger.Debug("init main");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        /// <summary>
        /// Creates host builder.
        /// </summary>
        /// <param name="args">Program arguments.</param>
        /// <returns>Host builder.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .UseNLog(new NLogAspNetCoreOptions() { RemoveLoggerFactoryFilter = true });
    }
}
