using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.DataAccess.Products
{
    /// <summary>
    /// Represents a DAO for Northwind product categories.
    /// </summary>
    public interface IProductCategoryDataAccessObject
    {
        /// <summary>
        /// Inserts a new Northwind product category to a data storage.
        /// </summary>
        /// <param name="productCategory">A <see cref="ProductCategoryTransferObject"/>.</param>
        /// <returns>A data storage identifier of a new product category.</returns>
        /// <exception cref="ArgumentNullException">Throw when product category is null.</exception>
        Task<int> InsertProductCategoryAsync(ProductCategoryTransferObject productCategory);

        /// <summary>
        /// Deletes a Northwind product category from a data storage.
        /// </summary>
        /// <param name="productCategoryId">An product category identifier.</param>
        /// <returns>True if a product category is deleted; otherwise false.</returns>
        /// <exception cref="ArgumentException">Throw when product category identifier is less than or equal to zero.</exception>
        Task<bool> DeleteProductCategoryAsync(int productCategoryId);

        /// <summary>
        /// Updates a Northwind product category in a data storage.
        /// </summary>
        /// <param name="productCategory">A <see cref="ProductCategoryTransferObject"/>.</param>
        /// <returns>True if a product category is updated; otherwise false.</returns>
        /// <exception cref="ArgumentNullException">Throw when product category is null.</exception>
        Task<bool> UpdateProductCategoryAsync(ProductCategoryTransferObject productCategory);

        /// <summary>
        /// Finds a Northwind product category using a specified identifier.
        /// </summary>
        /// <param name="productCategoryId">A data storage identifier of an existed product category.</param>
        /// <returns>A <see cref="ProductCategoryTransferObject"/> with specified identifier.</returns>
        /// <exception cref="ArgumentException">Throw when product category identifier is less than or equal to zero.</exception>
        /// <exception cref="ProductCategoryNotFoundException">Throw when product category is not found.</exception>
        Task<ProductCategoryTransferObject> FindProductCategoryAsync(int productCategoryId);

        /// <summary>
        /// Selects product categories using specified offset and limit.
        /// </summary>
        /// <param name="offset">An offset of the first object.</param>
        /// <param name="limit">A limit of returned objects.</param>
        /// <returns>A <see cref="IAsyncEnumerable{T}"/> of <see cref="ProductCategoryTransferObject"/>.</returns>
        /// <exception cref="ArgumentException">Throw when offset is less than or equal to zero.</exception>
        /// <exception cref="ArgumentException">Throw when limit is less than or equal to one.</exception>
        IAsyncEnumerable<ProductCategoryTransferObject> SelectProductCategoriesAsync(int offset, int limit);

        /// <summary>
        /// Selects all Northwind product categories with specified names.
        /// </summary>
        /// <param name="productCategoryNames">A <see cref="ICollection{T}"/> of product category names.</param>
        /// <returns>A <see cref="IAsyncEnumerable{T}"/> of <see cref="ProductCategoryTransferObject"/>.</returns>
        /// <exception cref="ArgumentNullException">Throw when product category names is null.</exception>
        /// <exception cref="ArgumentException">Throw when the count of product category names is less than or equal to one.</exception>
        IAsyncEnumerable<ProductCategoryTransferObject> SelectProductCategoriesByNameAsync(ICollection<string> productCategoryNames);
    }
}
