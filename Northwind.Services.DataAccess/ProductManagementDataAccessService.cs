using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Northwind.DataAccess;
using Northwind.DataAccess.Products;
using Northwind.Services.Products;

namespace Northwind.Services.DataAccess
{
    /// <summary>
    /// Represents a management service for products.
    /// </summary>
    public sealed class ProductManagementDataAccessService : IProductManagementService
    {
        private readonly NorthwindDataAccessFactory accessFactory;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductManagementDataAccessService"/> class.
        /// </summary>
        /// <param name="accessFactory">A <see cref="NorthwindDataAccessFactory"/>.</param>
        /// <param name="mapper">A <see cref="IMapper"/>.</param>
        /// <exception cref="ArgumentNullException">Throw when accessFactory or mapper is null.</exception>
        public ProductManagementDataAccessService(NorthwindDataAccessFactory accessFactory, IMapper mapper)
        {
            this.accessFactory = accessFactory ?? throw new ArgumentNullException(nameof(accessFactory));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc/>
        public async Task<int> CreateProductAsync(ProductModel product)
        {
            TaskArgumentVerificator.CheckItemIsNull(product);

            return await this.accessFactory
                    .GetProductDataAccessObject()
                    .InsertProductAsync(this.mapper.Map<ProductTransferObject>(product));
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyProductAsync(int productId)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, productId, "Must be greater than zero.");

            return await this.accessFactory
                    .GetProductDataAccessObject()
                    .DeleteProductAsync(productId);
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateProductAsync(int productId, ProductModel product)
        {
            TaskArgumentVerificator.CheckItemIsNull(product);
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, productId, "Must be greater than zero.");

            product.Id = productId;

            return await this.accessFactory
                    .GetProductDataAccessObject()
                    .UpdateProductAsync(this.mapper.Map<ProductTransferObject>(product));
        }

        /// <inheritdoc/>
        public async Task<ProductModel> GetProductAsync(int productId)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, productId, "Must be greater than zero.");

            try
            {
                return this.mapper.Map<ProductModel>(
                    await this.accessFactory
                    .GetProductDataAccessObject()
                    .FindProductAsync(productId));
            }
            catch (ProductNotFoundException)
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<ProductModel> GetProductsAsync(int offset, int limit)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 0, offset, "Must be greater than zero or equals zero.");
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 1, limit, "Must be greater than zero.");

            await foreach (var transfer in this.accessFactory
                .GetProductDataAccessObject()
                .SelectProductsAsync(offset, limit))
            {
                yield return this.mapper.Map<ProductModel>(transfer);
            }
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<ProductModel> LookupProductsByNameAsync(IList<string> names)
        {
            TaskArgumentVerificator.CheckItemIsNull(names);
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 1, names.Count, "Collection is empty.");

            await foreach (var transfer in this.accessFactory
                .GetProductDataAccessObject()
                .SelectProductsByNameAsync(names))
            {
                yield return this.mapper.Map<ProductModel>(transfer);
            }
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<ProductModel> GetProductsForCategoryAsync(IList<int> listOfCategoryId)
        {
            TaskArgumentVerificator.CheckItemIsNull(listOfCategoryId);
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 1, listOfCategoryId.Count, "Collection is empty.");

            await foreach (var transfer in this.accessFactory
                .GetProductDataAccessObject()
                .SelectProductsByCategoryAsync(listOfCategoryId))
            {
                yield return this.mapper.Map<ProductModel>(transfer);
            }
        }
    }
}
