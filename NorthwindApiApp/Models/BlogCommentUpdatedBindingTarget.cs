using System;
using Northwind.Services.Blogging;

namespace NorthwindApiApp.Models
{
    /// <summary>
    /// Represents a readed blog comment model.
    /// </summary>
    public class BlogCommentUpdatedBindingTarget
    {
        /// <summary>
        /// Gets or sets a blog comment text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Converts this to a new BlogCommentModel.
        /// </summary>
        /// <param name="blogArticleId">A blog article identifierg.</param>
        /// <param name="customerId">An identifier of the customer who published the comment to the blog.</param>
        /// <returns>A new BlogArticleModel.</returns>
        public BlogCommentModel ToBlogComment(int blogArticleId, int customerId) =>
            new BlogCommentModel()
            {
                BlogArticleId = blogArticleId,
                CustomerId = customerId,
                Text = this.Text,
                Posted = DateTime.Now,
            };
    }
}
