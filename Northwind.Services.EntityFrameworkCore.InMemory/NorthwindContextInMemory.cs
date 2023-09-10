using Microsoft.EntityFrameworkCore;
using Northwind.Services.Employees;
using Northwind.Services.Products;

namespace Northwind.Services.EntityFrameworkCore.InMemory
{
    /// <summary>
    /// Class NorthwindContext.
    /// </summary>
    public class NorthwindContextInMemory : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NorthwindContextInMemory"/> class.
        /// </summary>
        /// <param name="options">DbContextOptions.</param>
        public NorthwindContextInMemory(DbContextOptions<NorthwindContextInMemory> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets a product categories.
        /// </summary>
        public DbSet<ProductCategoryModel> ProductCategories { get; set; }

        /// <summary>
        /// Gets or sets a products.
        /// </summary>
        public DbSet<ProductModel> Products { get; set; }

        /// <summary>
        /// Gets or sets a employees.
        /// </summary>
        public DbSet<EmployeeModel> Employees { get; set; }
    }
}
