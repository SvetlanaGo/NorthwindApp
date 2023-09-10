namespace Northwind.Services.EntityFrameworkCore.Blogging.Entities
{
    /// <summary>
    /// Represents a blog article product.
    /// </summary>
    public class BlogArticleProduct
    {
        /// <summary>
        /// Gets or sets a blog article identifier.
        /// </summary>
        public int BlogArticleId { get; set; }

        /// <summary>
        /// Gets or sets a product identifier.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets a blog article.
        /// </summary>
        public virtual BlogArticle BlogArticle { get; set; }
    }
}
