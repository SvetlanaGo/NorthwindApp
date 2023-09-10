using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Northwind.Services.Products;

namespace Northwind.Services.EntityFrameworkCore.InMemory
{
    /// <summary>
    /// Represents a stub for a product category management service.
    /// </summary>
    public sealed class ProductCategoryManagementService : IProductCategoryManagementService
    {
        private readonly NorthwindContextInMemory context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoryManagementService"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <exception cref="ArgumentNullException">Throw when context is null.</exception>
        public ProductCategoryManagementService(NorthwindContextInMemory context) =>
            this.context = context ?? throw new ArgumentNullException(nameof(context));

        /// <inheritdoc/>
        public async Task<int> CreateCategoryAsync(ProductCategoryModel productCategory)
        {
            TaskArgumentVerificator.CheckItemIsNull(productCategory);

            productCategory.Id = this.context.Employees.Max(x => x.Id) + 1;
            await this.context.ProductCategories.AddAsync(productCategory);
            await this.context.SaveChangesAsync();

            return productCategory.Id;
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyCategoryAsync(int categoryId)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, categoryId, "Must be greater than zero.");

            var category = await this.context.ProductCategories.FindAsync(categoryId);
            if (category is null)
            {
                return false;
            }

            this.context.ProductCategories.Remove(category);
            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateCategoriesAsync(int categoryId, ProductCategoryModel productCategory)
        {
            TaskArgumentVerificator.CheckItemIsNull(productCategory);
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, categoryId, "Must be greater than zero.");

            var categoryUp = await this.context.ProductCategories.FindAsync(categoryId);
            if (categoryUp is null)
            {
                return false;
            }

            categoryUp.Name = productCategory.Name;
            categoryUp.Description = productCategory.Description;
            categoryUp.Picture = productCategory.Picture;

            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        /// <inheritdoc/>
        public async Task<ProductCategoryModel> GetCategoryAsync(int categoryId)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, categoryId, "Must be greater than zero.");

            return await this.context.ProductCategories.FindAsync(categoryId);
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<ProductCategoryModel> GetCategoriesAsync(int offset, int limit)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 0, offset, "Must be greater than zero or equals zero.");
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 1, limit, "Must be greater than zero.");

            await foreach (var productCategory in this.context.ProductCategories.Skip(offset).Take(limit).OrderBy(pc => pc.Id).AsAsyncEnumerable())
            {
                yield return productCategory;
            }
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<ProductCategoryModel> LookupCategoriesByNameAsync(IList<string> names)
        {
            TaskArgumentVerificator.CheckItemIsNull(names);
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 1, names.Count, "Collection is empty.");

            await foreach (var productCategory in this.context.ProductCategories.Where(pc => names.Contains(pc.Name)).OrderBy(pc => pc.Id).AsAsyncEnumerable())
            {
                yield return productCategory;
            }
        }
    }
}
