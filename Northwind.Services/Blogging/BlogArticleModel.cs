using System;

namespace Northwind.Services.Blogging
{
    /// <summary>
    /// Represents a blog article.
    /// </summary>
    public class BlogArticleModel
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
    }
}
