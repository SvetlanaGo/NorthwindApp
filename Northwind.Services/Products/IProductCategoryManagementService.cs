using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Services.Products
{
    /// <summary>
    /// Represents a management service for product categories.
    /// </summary>
    public interface IProductCategoryManagementService
    {
        /// <summary>
        /// Creates a new product category.
        /// </summary>
        /// <param name="productCategory">A <see cref="ProductCategoryModel"/> to create.</param>
        /// <returns>An identifier of a created product category.</returns>
        /// <exception cref="ArgumentNullException">Throw when product category is null.</exception>
        Task<int> CreateCategoryAsync(ProductCategoryModel productCategory);

        /// <summary>
        /// Destroys an existed product category.
        /// </summary>
        /// <param name="categoryId">A product category identifier.</param>
        /// <returns>True if a product category is destroyed; otherwise false.</returns>
        /// <exception cref="ArgumentException">Throw when product category identifier is less than or equal to zero.</exception>
        Task<bool> DestroyCategoryAsync(int categoryId);

        /// <summary>
        /// Updates a product category.
        /// </summary>
        /// <param name="categoryId">A product category identifier.</param>
        /// <param name="productCategory">A <see cref="ProductCategoryModel"/>.</param>
        /// <returns>True if a product category is updated; otherwise false.</returns>
        /// <exception cref="ArgumentNullException">Throw when product category is null.</exception>
        /// <exception cref="ArgumentException">Throw when product category identifier is less than or equal to zero.</exception>
        Task<bool> UpdateCategoriesAsync(int categoryId, ProductCategoryModel productCategory);

        /// <summary>
        /// Try to show a product category with specified identifier.
        /// </summary>
        /// <param name="categoryId">A product category identifier.</param>
        /// <returns>A product category.</returns>
        /// <exception cref="ArgumentException">Throw when product category identifier is less than or equal to zero.</exception>
        Task<ProductCategoryModel> GetCategoryAsync(int categoryId);

        /// <summary>
        /// Shows a list of product categories using specified offset and limit for pagination.
        /// </summary>
        /// <param name="offset">An offset of the first element to return.</param>
        /// <param name="limit">A limit of elements to return.</param>
        /// <returns>A <see cref="IAsyncEnumerable{T}"/> of <see cref="ProductCategoryModel"/>.</returns>
        /// <exception cref="ArgumentException">Throw when offset is less than or equal to zero.</exception>
        /// <exception cref="ArgumentException">Throw when limit is less than or equal to one.</exception>
        IAsyncEnumerable<ProductCategoryModel> GetCategoriesAsync(int offset, int limit);

        /// <summary>
        /// Looks up for product categories with specified names.
        /// </summary>
        /// <param name="names">A list of product category names.</param>
        /// <returns>A list of product categories with specified names.</returns>
        /// <exception cref="ArgumentNullException">Throw when product category names is null.</exception>
        /// <exception cref="ArgumentException">Throw when the count of product category names is less than or equal to one.</exception>
        IAsyncEnumerable<ProductCategoryModel> LookupCategoriesByNameAsync(IList<string> names);
    }
}
