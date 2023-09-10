using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Services.Products
{
    /// <summary>
    /// Represents a management service for products.
    /// </summary>
    public interface IProductManagementService
    {
        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="product">A <see cref="ProductModel"/> to create.</param>
        /// <returns>An identifier of a created product.</returns>
        /// <exception cref="ArgumentNullException">Throw when product is null.</exception>
        Task<int> CreateProductAsync(ProductModel product);

        /// <summary>
        /// Destroys an existed product.
        /// </summary>
        /// <param name="productId">A product identifier.</param>
        /// <returns>True if a product is destroyed; otherwise false.</returns>
        /// <exception cref="ArgumentException">Throw when product identifier is less than or equal to zero.</exception>
        Task<bool> DestroyProductAsync(int productId);

        /// <summary>
        /// Updates a product.
        /// </summary>
        /// <param name="productId">A product identifier.</param>
        /// <param name="product">A <see cref="ProductModel"/>.</param>
        /// <returns>True if a product is updated; otherwise false.</returns>
        /// <exception cref="ArgumentNullException">Throw when product is null.</exception>
        /// <exception cref="ArgumentException">Throw when product identifier is less than or equal to zero.</exception>
        Task<bool> UpdateProductAsync(int productId, ProductModel product);

        /// <summary>
        /// Try to show a product with specified identifier.
        /// </summary>
        /// <param name="productId">A product identifier.</param>
        /// <returns>Returns a product.</returns>
        /// <exception cref="ArgumentException">Throw when product identifier is less than or equal to zero.</exception>
        Task<ProductModel> GetProductAsync(int productId);

        /// <summary>
        /// Shows a list of products using specified offset and limit for pagination.
        /// </summary>
        /// <param name="offset">An offset of the first element to return.</param>
        /// <param name="limit">A limit of elements to return.</param>
        /// <returns>A <see cref="IAsyncEnumerable{T}"/> of <see cref="ProductModel"/>.</returns>
        /// <exception cref="ArgumentException">Throw when offset is less than or equal to zero.</exception>
        /// <exception cref="ArgumentException">Throw when limit is less than or equal to one.</exception>
        IAsyncEnumerable<ProductModel> GetProductsAsync(int offset, int limit);

        /// <summary>
        /// Looks up for product with specified names.
        /// </summary>
        /// <param name="names">A list of product names.</param>
        /// <returns>A list of products with specified names.</returns>
        /// <exception cref="ArgumentNullException">Throw when product names is null.</exception>
        /// <exception cref="ArgumentException">Throw when the count of product names is less than or equal to one.</exception>
        IAsyncEnumerable<ProductModel> LookupProductsByNameAsync(IList<string> names);

        /// <summary>
        /// Shows a list of products that belongs to a specified category.
        /// </summary>
        /// <param name="listOfCategoryId">A product category identifier.</param>
        /// <returns>A <see cref="IList{T}"/> of <see cref="ProductModel"/>.</returns>
        /// <exception cref="ArgumentNullException">Throw when collection of category identifier is null.</exception>
        /// <exception cref="ArgumentException">Throw when collection of category identifier is less than or equal to zero.</exception>
        IAsyncEnumerable<ProductModel> GetProductsForCategoryAsync(IList<int> listOfCategoryId);
    }
}
