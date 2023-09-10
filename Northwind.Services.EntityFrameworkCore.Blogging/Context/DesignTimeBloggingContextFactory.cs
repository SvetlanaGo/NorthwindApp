using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace Northwind.Services.EntityFrameworkCore.Blogging.Context
{
    /// <summary>
    /// Class DesignTimeBloggingContextFactory.
    /// </summary>
    public class DesignTimeBloggingContextFactory : IDesignTimeDbContextFactory<BloggingContext>
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DesignTimeBloggingContextFactory"/> class.
        /// </summary>
        public DesignTimeBloggingContextFactory()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DesignTimeBloggingContextFactory"/> class.
        /// </summary>
        /// <param name="logger">A logger.</param>
        /// <exception cref="ArgumentNullException">Throw when logger is null.</exception>
        public DesignTimeBloggingContextFactory(ILogger<DesignTimeBloggingContextFactory> logger) =>
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Creates a DbContext.
        /// </summary>
        /// <param name="args">Arguments.</param>
        /// <returns>A BloggingContext.</returns>
        public BloggingContext CreateDbContext(string[] args)
        {
            const string connectionStringName = "NORTHWIND_BLOGGING";
            const string connectioStringPrefix = "SQLCONNSTR_";

            string connectionString = Environment.GetEnvironmentVariable($"{connectioStringPrefix}{connectionStringName}");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException($"{connectioStringPrefix}{connectionStringName} environment variable is not set.");
            }

            this.logger?.LogInformation($"Using {connectioStringPrefix}{connectionStringName} environment variable as a connection string.");

            var builderOptions = new DbContextOptionsBuilder<BloggingContext>().UseSqlServer(connectionString).Options;

            return new BloggingContext(builderOptions);
        }
    }
}
