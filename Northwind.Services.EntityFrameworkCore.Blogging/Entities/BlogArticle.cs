using System;
using System.Collections.Generic;

namespace Northwind.Services.EntityFrameworkCore.Blogging.Entities
{
    /// <summary>
    /// Represents a blog article.
    /// </summary>
    public class BlogArticle
    {
        /// <summary>
        /// Gets or sets a blog article identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a blog article text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets a date and time of publication of the article.
        /// </summary>
        public DateTime Posted { get; set; }

        /// <summary>
        /// Gets or sets an identifier of the employee who published the article in the blog.
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// Gets or sets a blog article products.
        /// </summary>
#pragma warning disable CA2227 // Collection properties should be read only
        public virtual ICollection<BlogArticleProduct> BlogArticleProducts { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only

        /// <summary>
        /// Gets or sets a blog article comments.
        /// </summary>
#pragma warning disable CA2227 // Collection properties should be read only
        public virtual ICollection<BlogComment> BlogComments { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
    }
}
