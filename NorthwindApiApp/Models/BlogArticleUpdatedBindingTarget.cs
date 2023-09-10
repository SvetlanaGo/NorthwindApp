using System;
using Northwind.Services.Blogging;

namespace NorthwindApiApp.Models
{
    /// <summary>
    /// Represents a updated blog article model.
    /// </summary>
    public class BlogArticleUpdatedBindingTarget
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
        /// Converts this to a new BlogArticleModel.
        /// </summary>
        /// <param name="authorId">An identifier of the employee who published the article in the blog.</param>
        /// <returns>A new BlogArticleModel.</returns>
        public BlogArticleModel ToBlogArticle(int authorId) =>
            new BlogArticleModel()
            {
                Title = this.Title,
                Text = this.Text,
                Posted = DateTime.Now,
                AuthorId = authorId,
            };
    }
}
