using System;
using Northwind.Services.Blogging;

namespace NorthwindApiApp.Models
{
    /// <summary>
    /// Represents a created blog comment model.
    /// </summary>
    public class BlogCommentCreatedBindingTarget
    {
        /// <summary>
        /// Gets or sets an identifier of the customer who published the comment to the blog.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets a blog comment text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Converts this to a new BlogCommentModel.
        /// </summary>
        /// <param name="articleId">A blog article identifier.</param>
        /// <returns>A created blog comment.</returns>
        public BlogCommentModel ToBlogArticle(int articleId) =>
            new BlogCommentModel()
            {
                BlogArticleId = articleId,
                CustomerId = this.CustomerId,
                Text = this.Text,
                Posted = DateTime.Now,
            };
    }
}
