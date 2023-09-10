using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Services.Blogging
{
    /// <summary>
    /// Represents a management service for blog comments.
    /// </summary>
    public interface IBlogCommentService
    {
        /// <summary>
        /// Creates a new blog comment.
        /// </summary>
        /// <param name="blogArticleId">A blog article identifier.</param>
        /// <param name="blogComment">A <see cref="BlogCommentModel"/> to create.</param>
        /// <returns>An identifier of a created blog comment.</returns>
        /// <exception cref="ArgumentNullException">Throw when blog comment is null.</exception>
        Task<int> CreateBlogCommentAsync(int blogArticleId, BlogCommentModel blogComment);

        /// <summary>
        /// Destroys an existed blog comment.
        /// </summary>
        /// <param name="blogArticleId">A blog article identifier.</param>
        /// <param name="blogCommentId">A blog comment identifier.</param>
        /// <returns>True if a blog article is destroyed; otherwise false.</returns>
        /// <exception cref="ArgumentException">Throw when blog article identifier or blog comment identifier is less than or equal to zero.</exception>
        Task<bool> DestroyBlogCommentAsync(int blogArticleId, int blogCommentId);

        /// <summary>
        /// Updates a blog comment.
        /// </summary>
        /// <param name="blogCommentId">A blog comment identifier.</param>
        /// <param name="blogComment">A <see cref="BlogCommentModel"/>.</param>
        /// <returns>True if a blog comment is updated; otherwise false.</returns>
        /// <exception cref="ArgumentNullException">Throw when blog comment is null.</exception>
        /// <exception cref="ArgumentException">Throw when blog comment identifier is less than or equal to zero.</exception>
        Task<bool> UpdateBlogCommentAsync(int blogCommentId, BlogCommentModel blogComment);

        /// <summary>
        /// Try to show a blog comment with specified identifier.
        /// </summary>
        /// <param name="blogArticleId">A blog article identifier.</param>
        /// <param name="blogCommentId">A blog comment identifier.</param>
        /// <returns>Returns an blog comment.</returns>
        /// <exception cref="ArgumentException">Throw when blogArticleId or blogCommentId is less than or equal to zero.</exception>
        Task<BlogCommentModel> GetBlogCommentAsync(int blogArticleId, int blogCommentId);

        /// <summary>
        /// Shows a list of blog comments using specified offset and limit for pagination.
        /// </summary>
        /// <param name="blogArticleId">A blog article identifier.</param>
        /// <param name="offset">An offset of the first element to return.</param>
        /// <param name="limit">A limit of elements to return.</param>
        /// <returns>A <see cref="IAsyncEnumerable{T}"/> of <see cref="BlogCommentModel"/>.</returns>
        /// <exception cref="ArgumentException">Throw when blogArticleId or blogCommentId is less than or equal to zero.</exception>
        /// <exception cref="ArgumentException">Throw when offset is less than or equal to zero.</exception>
        /// <exception cref="ArgumentException">Throw when limit is less than or equal to one.</exception>
        IAsyncEnumerable<BlogCommentModel> GetBlogCommentsAsync(int blogArticleId, int offset, int limit);
    }
}
