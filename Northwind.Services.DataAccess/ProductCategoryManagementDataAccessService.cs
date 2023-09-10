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
    /// Represents a management service for product categories.
    /// </summary>
    public sealed class ProductCategoryManagementDataAccessService : IProductCategoryManagementService
    {
        private readonly NorthwindDataAccessFactory accessFactory;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoryManagementDataAccessService"/> class.
        /// </summary>
        /// <param name="accessFactory">A <see cref="NorthwindDataAccessFactory"/>.</param>
        /// <param name="mapper">A <see cref="IMapper"/>.</param>
        /// <exception cref="ArgumentNullException">Throw when accessFactory or mapper is null.</exception>
        public ProductCategoryManagementDataAccessService(NorthwindDataAccessFactory accessFactory, IMapper mapper)
        {
            this.accessFactory = accessFactory ?? throw new ArgumentNullException(nameof(accessFactory));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc/>
        public async Task<int> CreateCategoryAsync(ProductCategoryModel productCategory)
        {
            TaskArgumentVerificator.CheckItemIsNull(productCategory);

            return await this.accessFactory
                    .GetProductCategoryDataAccessObject()
                    .InsertProductCategoryAsync(this.mapper.Map<ProductCategoryTransferObject>(productCategory));
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyCategoryAsync(int categoryId)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, categoryId, "Must be greater than zero.");

            return await this.accessFactory
                    .GetProductCategoryDataAccessObject()
                    .DeleteProductCategoryAsync(categoryId);
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateCategoriesAsync(int categoryId, ProductCategoryModel productCategory)
        {
            TaskArgumentVerificator.CheckItemIsNull(productCategory);
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, categoryId, "Must be greater than zero.");

            productCategory.Id = categoryId;

            return await this.accessFactory
                    .GetProductCategoryDataAccessObject()
                    .UpdateProductCategoryAsync(this.mapper.Map<ProductCategoryTransferObject>(productCategory));
        }

        /// <inheritdoc/>
        public async Task<ProductCategoryModel> GetCategoryAsync(int categoryId)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, categoryId, "Must be greater than zero.");

            try
            {
                return this.mapper.Map<ProductCategoryModel>(
                    await this.accessFactory
                    .GetProductCategoryDataAccessObject()
                    .FindProductCategoryAsync(categoryId));
            }
            catch (ProductNotFoundException)
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<ProductCategoryModel> GetCategoriesAsync(int offset, int limit)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 0, offset, "Must be greater than zero or equals zero.");
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 1, limit, "Must be greater than zero.");

            await foreach (var transfer in this.accessFactory
                .GetProductCategoryDataAccessObject()
                .SelectProductCategoriesAsync(offset, limit))
            {
                yield return this.mapper.Map<ProductCategoryModel>(transfer);
            }
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<ProductCategoryModel> LookupCategoriesByNameAsync(IList<string> names)
        {
            TaskArgumentVerificator.CheckItemIsNull(names);
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 1, names.Count, "Collection is empty.");

            await foreach (var transfer in this.accessFactory
                .GetProductCategoryDataAccessObject()
                .SelectProductCategoriesByNameAsync(names))
            {
                yield return this.mapper.Map<ProductCategoryModel>(transfer);
            }
        }
    }
}
