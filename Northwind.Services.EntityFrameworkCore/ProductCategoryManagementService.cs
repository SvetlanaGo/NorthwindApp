using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Northwind.Services.EntityFrameworkCore.Context;
using Northwind.Services.EntityFrameworkCore.Entities;
using Northwind.Services.Products;

namespace Northwind.Services.EntityFrameworkCore
{
    /// <summary>
    /// Represents a product category management service.
    /// </summary>
    public sealed class ProductCategoryManagementService : IProductCategoryManagementService
    {
        private readonly NorthwindContext context;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoryManagementService"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="mapper">A <see cref="IMapper"/>.</param>
        /// <exception cref="ArgumentNullException">Throw when context is null.</exception>
        public ProductCategoryManagementService(NorthwindContext context, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc/>
        public async Task<int> CreateCategoryAsync(ProductCategoryModel productCategory)
        {
            TaskArgumentVerificator.CheckItemIsNull(productCategory);

            var transfer = this.mapper.Map<Category>(productCategory);
            await this.context.Categories.AddAsync(transfer);
            await this.context.SaveChangesAsync();

            return transfer.CategoryId;
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyCategoryAsync(int categoryId)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, categoryId, "Must be greater than zero.");

            var category = await this.context.Categories.FindAsync(categoryId);
            if (category is null)
            {
                return false;
            }

            this.context.Categories.Remove(category);
            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateCategoriesAsync(int categoryId, ProductCategoryModel productCategory)
        {
            TaskArgumentVerificator.CheckItemIsNull(productCategory);
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, categoryId, "Must be greater than zero.");

            var categoryUp = await this.context.Categories.FindAsync(categoryId);
            if (categoryUp is null)
            {
                return false;
            }

            categoryUp.CategoryName = productCategory.Name;
            categoryUp.Description = productCategory.Description;
            categoryUp.Picture = productCategory.Picture;

            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        /// <inheritdoc/>
        public async Task<ProductCategoryModel> GetCategoryAsync(int categoryId)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, categoryId, "Must be greater than zero.");

            return this.mapper.Map<ProductCategoryModel>(await this.context.Categories.FindAsync(categoryId));
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<ProductCategoryModel> GetCategoriesAsync(int offset, int limit)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 0, offset, "Must be greater than zero or equals zero.");
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 1, limit, "Must be greater than zero.");

            await foreach (var productCategory in this.context.Categories.OrderBy(pc => pc.CategoryId).Skip(offset).Take(limit).AsAsyncEnumerable())
            {
                yield return this.mapper.Map<ProductCategoryModel>(productCategory);
            }
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<ProductCategoryModel> LookupCategoriesByNameAsync(IList<string> names)
        {
            TaskArgumentVerificator.CheckItemIsNull(names);
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 1, names.Count, "Collection is empty.");

            await foreach (var productCategory in this.context.Categories.Where(pc => names.Contains(pc.CategoryName)).OrderBy(pc => pc.CategoryId).AsAsyncEnumerable())
            {
                yield return this.mapper.Map<ProductCategoryModel>(productCategory);
            }
        }
    }
}
