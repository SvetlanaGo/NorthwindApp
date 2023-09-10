using System;
using Northwind.Services.Blogging;

namespace NorthwindApiApp.Models
{
    /// <summary>
    /// Represents a created blog article model.
    /// </summary>
    public class BlogArticleCreatedBindingTarget
    {
        /// <summary>
        /// Gets or sets a title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a blog article text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets an identifier of the employee who published the article in the blog.
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// Converts this to a new BlogArticleModel.
        /// </summary>
        /// <returns>A created blog article.</returns>
        public BlogArticleModel ToBlogArticle() =>
            new BlogArticleModel()
            {
                Title = this.Title,
                Text = this.Text,
                Posted = DateTime.Now,
                AuthorId = this.AuthorId,
            };
    }
}
