using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Northwind.Services.Products;

namespace Northwind.Services.EntityFrameworkCore.InMemory
{
    /// <summary>
    /// Represents a stub for a product management service.
    /// </summary>
    public sealed class ProductManagementService : IProductManagementService
    {
        private readonly NorthwindContextInMemory context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductManagementService"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <exception cref="ArgumentNullException">Throw when context is null.</exception>
        public ProductManagementService(NorthwindContextInMemory context) =>
            this.context = context ?? throw new ArgumentNullException(nameof(context));

        /// <inheritdoc/>
        public async Task<int> CreateProductAsync(ProductModel product)
        {
            TaskArgumentVerificator.CheckItemIsNull(product);

            product.Id = this.context.Employees.Max(x => x.Id) + 1;
            await this.context.Products.AddAsync(product);
            await this.context.SaveChangesAsync();

            return product.Id;
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyProductAsync(int productId)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, productId, "Must be greater than zero.");

            var product = await this.context.Products.FindAsync(productId);
            if (product is null)
            {
                return false;
            }

            this.context.Products.Remove(product);
            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateProductAsync(int productId, ProductModel product)
        {
            TaskArgumentVerificator.CheckItemIsNull(product);
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, productId, "Must be greater than zero.");

            var productUp = await this.context.Products.FindAsync(productId);
            if (productUp is null)
            {
                return false;
            }

            productUp.Name = product.Name;
            productUp.SupplierId = product.SupplierId;
            productUp.CategoryId = product.CategoryId;
            productUp.QuantityPerUnit = product.QuantityPerUnit;
            productUp.UnitPrice = product.UnitPrice;
            productUp.UnitsInStock = product.UnitsInStock;
            productUp.UnitsOnOrder = product.UnitsOnOrder;
            productUp.ReorderLevel = product.ReorderLevel;
            productUp.Discontinued = product.Discontinued;

            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        /// <inheritdoc/>
        public async Task<ProductModel> GetProductAsync(int productId)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, productId, "Must be greater than zero.");

            return await this.context.Products.FindAsync(productId);
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<ProductModel> GetProductsAsync(int offset, int limit)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 0, offset, "Must be greater than zero or equals zero.");
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 1, limit, "Must be greater than zero.");

            await foreach (var product in this.context.Products.OrderBy(p => p.Id).Skip(offset).Take(limit).AsAsyncEnumerable())
            {
                yield return product;
            }
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<ProductModel> LookupProductsByNameAsync(IList<string> names)
        {
            TaskArgumentVerificator.CheckItemIsNull(names);
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 1, names.Count, "Must be greater than zero.");

            await foreach (var product in this.context.Products.Where(p => names.Contains(p.Name)).OrderBy(p => p.Id).AsAsyncEnumerable())
            {
                yield return product;
            }
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<ProductModel> GetProductsForCategoryAsync(IList<int> listOfCategoryId)
        {
            TaskArgumentVerificator.CheckItemIsNull(listOfCategoryId);
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 1, listOfCategoryId.Count, "Must be greater than zero.");

            await foreach (var product in this.context.Products.Where(p => p.CategoryId != null && listOfCategoryId.Contains((int)p.CategoryId)).AsAsyncEnumerable())
            {
                yield return product;
            }
        }
    }
}
