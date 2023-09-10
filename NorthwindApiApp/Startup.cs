using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Northwind.Services.Blogging;
using Northwind.Services.EntityFrameworkCore.Blogging;
using Northwind.Services.EntityFrameworkCore.Blogging.Context;

namespace NorthwindApiApp
{
    /// <summary>
    /// Class Startup.
    /// </summary>
    public class Startup
    {
        private readonly OperationMode mode;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.mode = GetOperationMode(this.Configuration["OperationMode"]?.ToUpperInvariant());
        }

        /// <summary>
        /// Gets configuration root.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Service collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = this.Configuration.GetConnectionString("SqlConnection");
            switch (this.mode)
            {
                case OperationMode.EntityFramework:
                    services.AddEntityFrameworkService(connectionString);
                    break;
                case OperationMode.Sql:
                    services.AddSqlService(connectionString);
                    break;
                default:
                    services.AddMemoryService();
                    break;
            }

            services
                .Configure<JsonOptions>(opts =>
                    {
                        opts.JsonSerializerOptions.IgnoreNullValues = true;
                    })
                .AddScoped<IBloggingService, BloggingService>()
                .AddScoped<IBlogArticleProductService, BlogArticleProductService>()
                .AddScoped<IBlogCommentService, BlogCommentService>()
                .AddScoped<IDesignTimeDbContextFactory<BloggingContext>, DesignTimeBloggingContextFactory>()
                .AddSingleton(m => new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MappingProfile());
                }).CreateMapper())
                .AddSwaggerGen(c =>
                    {
                        c.SwaggerDoc("v1", new OpenApiInfo { Title = "NorthwindApiApp", Version = "v1" });
                        c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                    })
                .AddControllers(opt => opt.SuppressAsyncSuffixInActionNames = false);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">ApplicationBuilder.</param>
        /// <param name="env">WebHostEnvironment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NorthwindApiApp v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (this.mode == OperationMode.InMemory)
            {
                SeedData.GenerateSeedData(app, this.Configuration);
            }
        }

        private static OperationMode GetOperationMode(string mode) => mode switch
        {
            "ENTITYFRAMEWORK" => OperationMode.EntityFramework,
            "MEMORY" => OperationMode.InMemory,
            "SQL" => OperationMode.Sql,
            _ => OperationMode.EntityFramework,
        };
    }
}
