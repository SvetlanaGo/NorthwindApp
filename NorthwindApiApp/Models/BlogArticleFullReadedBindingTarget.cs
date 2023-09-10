using System;
using System.Globalization;
using Northwind.Services.Blogging;

namespace NorthwindApiApp.Models
{
    /// <summary>
    /// Represents a full readed blog article model.
    /// </summary>
    public class BlogArticleFullReadedBindingTarget
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlogArticleFullReadedBindingTarget"/> class.
        /// </summary>
        /// <param name="blogArticleModel">Blog article model.</param>
        /// <param name="authorName">Author name.</param>
        /// <exception cref="ArgumentNullException">Throw when blog articles model is null.</exception>
        public BlogArticleFullReadedBindingTarget(BlogArticleModel blogArticleModel, string authorName)
        {
            if (blogArticleModel is null)
            {
                throw new ArgumentNullException(nameof(blogArticleModel));
            }

            this.Id = blogArticleModel.Id;
            this.Title = blogArticleModel.Title;
            this.Posted = blogArticleModel.Posted.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture);
            this.AuthorId = blogArticleModel.AuthorId;
            this.AuthorName = authorName;
            this.Text = blogArticleModel.Text;
        }

        /// <summary>
        /// Gets or sets a blog article identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a date and time of publication of the article.
        /// </summary>
        public string Posted { get; set; }

        /// <summary>
        /// Gets or sets an identifier of the employee who published the article in the blog.
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// Gets or sets a name of the employee who published the article in the blog.
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// Gets or sets a blog article text.
        /// </summary>
        public string Text { get; set; }
    }
}
