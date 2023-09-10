using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Services.Blogging
{
    /// <summary>
    /// Represents a management service for blog articles.
    /// </summary>
    public interface IBloggingService
    {
        /// <summary>
        /// Creates a new blog article.
        /// </summary>
        /// <param name="blogArticle">A <see cref="BlogArticleModel"/> to create.</param>
        /// <returns>An identifier of a created blog article.</returns>
        /// <exception cref="ArgumentNullException">Throw when blog article is null.</exception>
        Task<int> CreateBlogArticleAsync(BlogArticleModel blogArticle);

        /// <summary>
        /// Destroys an existed blog article.
        /// </summary>
        /// <param name="blogArticleId">A blog article identifier.</param>
        /// <returns>True if a blog article is destroyed; otherwise false.</returns>
        /// <exception cref="ArgumentException">Throw when blog article identifier is less than or equal to zero.</exception>
        Task<bool> DestroyBlogArticleAsync(int blogArticleId);

        /// <summary>
        /// Updates a blog article.
        /// </summary>
        /// <param name="blogArticleId">A blog article identifier.</param>
        /// <param name="blogArticle">A <see cref="BlogArticleModel"/>.</param>
        /// <returns>True if a blog article is updated; otherwise false.</returns>
        /// <exception cref="ArgumentNullException">Throw when blog article is null.</exception>
        /// <exception cref="ArgumentException">Throw when blog article identifier is less than or equal to zero.</exception>
        Task<bool> UpdateBlogArticleAsync(int blogArticleId, BlogArticleModel blogArticle);

        /// <summary>
        /// Try to show a blog article with specified identifier.
        /// </summary>
        /// <param name="blogArticleId">A blog article identifier.</param>
        /// <returns>Returns an blog article.</returns>
        /// <exception cref="ArgumentException">Throw when employeeId is less than or equal to zero.</exception>
        Task<BlogArticleModel> GetBlogArticleAsync(int blogArticleId);

        /// <summary>
        /// Shows a list of blog articles using specified offset and limit for pagination.
        /// </summary>
        /// <param name="offset">An offset of the first element to return.</param>
        /// <param name="limit">A limit of elements to return.</param>
        /// <returns>A <see cref="IAsyncEnumerable{T}"/> of <see cref="BlogArticleModel"/>.</returns>
        /// <exception cref="ArgumentException">Throw when offset is less than or equal to zero.</exception>
        /// <exception cref="ArgumentException">Throw when limit is less than or equal to one.</exception>
        IAsyncEnumerable<BlogArticleModel> GetBlogArticlesAsync(int offset, int limit);
    }
}
