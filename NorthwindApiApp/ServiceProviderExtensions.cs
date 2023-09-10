using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Northwind.DataAccess;
using Northwind.DataAccess.SqlServer;
using Northwind.Services.DataAccess;
using Northwind.Services.Employees;
using Northwind.Services.Products;
using EFService = Northwind.Services.EntityFrameworkCore;
using MemoryService = Northwind.Services.EntityFrameworkCore.InMemory;

namespace NorthwindApiApp
{
    /// <summary>
    /// Class ServiceProviderExtensions.
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Add memory service.
        /// </summary>
        /// <param name="services">Services.</param>
        public static void AddMemoryService(this IServiceCollection services) =>
            services
                .AddTransient<IProductManagementService, MemoryService.ProductManagementService>()
                .AddTransient<IProductCategoryManagementService, MemoryService.ProductCategoryManagementService>()
                .AddTransient<IProductCategoryPicturesManagementService, MemoryService.ProductCategoryPicturesManagementService>()
                .AddTransient<IEmployeeManagementService, MemoryService.EmployeeManagementService>()
                .AddDbContext<MemoryService.NorthwindContextInMemory>(opt => opt.UseInMemoryDatabase("Northwind"));

        /// <summary>
        /// Add EntityFramework service.
        /// </summary>
        /// <param name="services">Services.</param>
        /// <param name="connectionString">ConnectionString.</param>
        public static void AddEntityFrameworkService(this IServiceCollection services, string connectionString) =>
            services
                .AddScoped<IProductManagementService, EFService.ProductManagementService>()
                .AddScoped<IProductCategoryManagementService, EFService.ProductCategoryManagementService>()
                .AddScoped<IProductCategoryPicturesManagementService, EFService.ProductCategoryPicturesManagementService>()
                .AddScoped<IEmployeeManagementService, EFService.EmployeeManagementService>()
                .AddDbContext<EFService.Context.NorthwindContext>(opt => opt.UseSqlServer(connectionString));

        /// <summary>
        /// Add sql service.
        /// </summary>
        /// <param name="services">Services.</param>
        /// <param name="connectionString">ConnectionString.</param>
        public static void AddSqlService(this IServiceCollection services, string connectionString) =>
            services
                .AddScoped<NorthwindDataAccessFactory, SqlServerDataAccessFactory>()
                .AddScoped<IProductManagementService, ProductManagementDataAccessService>()
                .AddScoped<IProductCategoryManagementService, ProductCategoryManagementDataAccessService>()
                .AddScoped<IProductCategoryPicturesManagementService, ProductCategoryPicturesManagementDataAccessService>()
                .AddScoped<IEmployeeManagementService, EmployeeManagementDataAccessService>()
                .AddScoped(provider => new SqlConnection(connectionString));
    }
}
