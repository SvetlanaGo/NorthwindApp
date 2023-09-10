using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Services.Blogging
{
    /// <summary>
    /// Represents a management service for blog articles.
    /// </summary>
    public interface IBlogArticleProductService
    {
        /// <summary>
        /// Create a link to a product for an article.
        /// </summary>
        /// <param name="blogArticleProduct">A blog article product.</param>
        /// <returns>An identifier of a created blog article product.</returns>
        /// <exception cref="ArgumentNullException">Throw when blog article product is null.</exception>
        Task<(int blogArticleId, int productId)> CreateBlogArticleProductAsync(BlogArticleProductModel blogArticleProduct);

        /// <summary>
        /// Remove an existed link to a product from an article.
        /// </summary>
        /// <param name="blogArticleProduct">A blog article product.</param>
        /// <returns>True if a blog article product is destroyed; otherwise false.</returns>
        /// <exception cref="ArgumentNullException">Throw when blog article product is null.</exception>
        Task<bool> DestroyBlogArticleProductAsync(BlogArticleProductModel blogArticleProduct);

        /// <summary>
        /// Try to show a product with specified identifier.
        /// </summary>
        /// <param name="blogArticleId">A blog article identifier.</param>
        /// <param name="productId">A product identifier.</param>
        /// <returns>Returns a blog article product.</returns>
        /// <exception cref="ArgumentException">Throw when blog article identifier or product identifier is less than or equal to zero.</exception>
        Task<BlogArticleProductModel> GetBlogArticleProductAsync(int blogArticleId, int productId);

        /// <summary>
        /// Return all related products.
        /// </summary>
        /// <param name="blogArticleId">A blog article identifier.</param>
        /// <returns>All related products.</returns>
        /// <exception cref="ArgumentException">Throw when blog article identifier is less than or equal to zero.</exception>
        IAsyncEnumerable<BlogArticleProductModel> GetBlogArticleProductsAsync(int blogArticleId);
    }
}
