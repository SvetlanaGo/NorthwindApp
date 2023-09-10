using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.DataAccess.Products
{
    /// <summary>
    /// Represents a DAO for Northwind products.
    /// </summary>
    public interface IProductDataAccessObject
    {
        /// <summary>
        /// Inserts a new Northwind product to a data storage.
        /// </summary>
        /// <param name="product">A <see cref="ProductTransferObject"/>.</param>
        /// <returns>A data storage identifier of a new product.</returns>
        /// <exception cref="ArgumentNullException">Throw when product is null.</exception>
        Task<int> InsertProductAsync(ProductTransferObject product);

        /// <summary>
        /// Deletes a Northwind product from a data storage.
        /// </summary>
        /// <param name="productId">An product identifier.</param>
        /// <returns>True if a product is deleted; otherwise false.</returns>
        /// <exception cref="ArgumentException">Throw when product identifier is less than or equal to zero.</exception>
        Task<bool> DeleteProductAsync(int productId);

        /// <summary>
        /// Updates a Northwind product in a data storage.
        /// </summary>
        /// <param name="product">A <see cref="ProductTransferObject"/>.</param>
        /// <returns>True if a product is updated; otherwise false.</returns>
        /// <exception cref="ArgumentNullException">Throw when product is null.</exception>
        Task<bool> UpdateProductAsync(ProductTransferObject product);

        /// <summary>
        /// Finds a Northwind product using a specified identifier.
        /// </summary>
        /// <param name="productId">A data storage identifier of an existed product.</param>
        /// <returns>A <see cref="ProductTransferObject"/> with specified identifier.</returns>
        /// <exception cref="ArgumentException">Throw when product identifier is less than or equal to zero.</exception>
        /// <exception cref="ProductNotFoundException">Throw when product is not found.</exception>
        Task<ProductTransferObject> FindProductAsync(int productId);

        /// <summary>
        /// Selects products using specified offset and limit.
        /// </summary>
        /// <param name="offset">An offset of the first object.</param>
        /// <param name="limit">A limit of returned objects.</param>
        /// <returns>A <see cref="IAsyncEnumerable{T}"/> of <see cref="ProductTransferObject"/>.</returns>
        /// <exception cref="ArgumentException">Throw when offset is less than or equal to zero.</exception>
        /// <exception cref="ArgumentException">Throw when limit is less than or equal to one.</exception>
        IAsyncEnumerable<ProductTransferObject> SelectProductsAsync(int offset, int limit);

        /// <summary>
        /// Selects all Northwind products with specified names.
        /// </summary>
        /// <param name="productNames">A <see cref="IEnumerable{T}"/> of product names.</param>
        /// <returns>A <see cref="IAsyncEnumerable{T}"/> of <see cref="ProductTransferObject"/>.</returns>
        /// <exception cref="ArgumentNullException">Throw when product names is null.</exception>
        /// <exception cref="ArgumentException">Throw when the count of product names is less than or equal to one.</exception>
        IAsyncEnumerable<ProductTransferObject> SelectProductsByNameAsync(ICollection<string> productNames);

        /// <summary>
        /// Selects all Northwind products that belongs to specified categories.
        /// </summary>
        /// <param name="collectionOfCategoryId">A <see cref="ICollection{T}"/> of category id.</param>
        /// <returns>A <see cref="IAsyncEnumerable{T}"/> of <see cref="ProductTransferObject"/>.</returns>
        /// <exception cref="ArgumentNullException">Throw when collection of category identifier is null.</exception>
        /// <exception cref="ArgumentException">Throw when the count of category identifier is less than or equal to one.</exception>
        IAsyncEnumerable<ProductTransferObject> SelectProductsByCategoryAsync(ICollection<int> collectionOfCategoryId);
    }
}
