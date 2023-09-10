using System;

namespace Northwind.Services.EntityFrameworkCore.Blogging.Entities
{
    /// <summary>
    /// Represents a blog comment.
    /// </summary>
    public class BlogComment
    {
        /// <summary>
        /// Gets or sets a blog comment identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a blog article identifier.
        /// </summary>
        public int BlogArticleId { get; set; }

        /// <summary>
        /// Gets or sets an identifier of the customer who published the comment to the blog.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets a blog comment text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets a date and time of publication of the comment.
        /// </summary>
        public DateTime Posted { get; set; }

        /// <summary>
        /// Gets or sets a blog article.
        /// </summary>
        public virtual BlogArticle BlogArticle { get; set; }
    }
}
