namespace Northwind.Services.Blogging
{
    /// <summary>
    /// Represents a blog article product.
    /// </summary>
    public class BlogArticleProductModel
    {
        /// <summary>
        /// Gets or sets a blog article identifier.
        /// </summary>
        public int BlogArticleId { get; set; }

        /// <summary>
        /// Gets or sets a product identifier.
        /// </summary>
        public int ProductId { get; set; }
    }
}
