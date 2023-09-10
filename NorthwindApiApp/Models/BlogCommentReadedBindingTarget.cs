using System;
using System.Globalization;
using Northwind.Services.Blogging;

namespace NorthwindApiApp.Models
{
    /// <summary>
    /// Represents a readed blog comment model.
    /// </summary>
    public class BlogCommentReadedBindingTarget
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlogCommentReadedBindingTarget"/> class.
        /// </summary>
        /// <param name="blogCommentModel">Blog article model.</param>
        /// <exception cref="ArgumentNullException">Throw when blog articles model is null.</exception>
        public BlogCommentReadedBindingTarget(BlogCommentModel blogCommentModel)
        {
            if (blogCommentModel is null)
            {
                throw new ArgumentNullException(nameof(blogCommentModel));
            }

            this.Id = blogCommentModel.Id;
            this.BlogArticleId = blogCommentModel.BlogArticleId;
            this.CustomerId = blogCommentModel.CustomerId;
            this.Text = blogCommentModel.Text;
            this.Posted = blogCommentModel.Posted.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture);
        }

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
        public string Posted { get; set; }
    }
}
