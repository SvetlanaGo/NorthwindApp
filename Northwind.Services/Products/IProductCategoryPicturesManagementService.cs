using System;
using System.IO;
using System.Threading.Tasks;

namespace Northwind.Services.Products
{
    /// <summary>
    /// Represents a management service for product category pictures.
    /// </summary>
    public interface IProductCategoryPicturesManagementService
    {
        /// <summary>
        /// Destroy a product category picture.
        /// </summary>
        /// <param name="categoryId">A product category identifier.</param>
        /// <returns>True if a product category is exist; otherwise false.</returns>
        /// <exception cref="ArgumentException">Throw when product category identifier is less than or equal to zero.</exception>
        Task<bool> DestroyPictureAsync(int categoryId);

        /// <summary>
        /// Update a product category picture.
        /// </summary>
        /// <param name="categoryId">A product category identifier.</param>
        /// <param name="stream">A <see cref="Stream"/>.</param>
        /// <returns>True if a product category is exist; otherwise false.</returns>
        /// <exception cref="ArgumentNullException">Throw when stream is null.</exception>
        /// <exception cref="ArgumentException">Throw when product category identifier is less than or equal to zero.</exception>
        Task<bool> UpdatePictureAsync(int categoryId, Stream stream);

        /// <summary>
        /// Try to show a product category picture.
        /// </summary>
        /// <param name="categoryId">A product category identifier.</param>
        /// <returns>Product category picture.</returns>
        /// <exception cref="ArgumentException">Throw when product category identifier is less than or equal to zero.</exception>
        Task<byte[]> GetPictureAsync(int categoryId);
    }
}
