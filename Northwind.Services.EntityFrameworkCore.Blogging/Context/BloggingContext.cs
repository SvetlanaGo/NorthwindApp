using System;
using Microsoft.EntityFrameworkCore;
using Northwind.Services.EntityFrameworkCore.Blogging.Entities;

namespace Northwind.Services.EntityFrameworkCore.Blogging.Context
{
    /// <summary>
    /// Class BloggingContext.
    /// </summary>
    public class BloggingContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BloggingContext"/> class.
        /// </summary>
        public BloggingContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BloggingContext"/> class.
        /// </summary>
        /// <param name="options">DbContextOptions.</param>
        public BloggingContext(DbContextOptions<BloggingContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets a BlogArticles.
        /// </summary>
        public DbSet<BlogArticle> BlogArticles { get; set; }

        /// <summary>
        /// Gets or sets a BlogArticleProducts.
        /// </summary>
        public DbSet<BlogArticleProduct> BlogArticleProducts { get; set; }

        /// <summary>
        /// Gets or sets a BlogComments.
        /// </summary>
        public DbSet<BlogComment> BlogComments { get; set; }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<BlogArticle>(entity =>
            {
                entity.HasKey(b => b.Id);

                entity.Property(b => b.Id).HasColumnType("int").HasColumnName("blog_article_id");
                entity.Property(b => b.Title).HasColumnType("nvarchar(50)").HasColumnName("title");
                entity.Property(b => b.Text).HasColumnType("nvarchar(2000)").HasColumnName("text");
                entity.Property(b => b.Posted).HasColumnType("datetime").HasColumnName("publication_date");
                entity.Property(b => b.AuthorId).HasColumnType("int").HasColumnName("author_id");

                entity.ToTable("Articles");
            });

            modelBuilder.Entity<BlogArticleProduct>(entity =>
            {
                entity.HasKey(b => new { b.BlogArticleId, b.ProductId });

                entity.HasOne(d => d.BlogArticle)
                    .WithMany(p => p.BlogArticleProducts)
                    .HasForeignKey(d => d.BlogArticleId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ArticleProducts_Articles");

                entity.Property(b => b.BlogArticleId).HasColumnType("int").HasColumnName("blog_article_id");
                entity.Property(b => b.ProductId).HasColumnType("int").HasColumnName("product_id");

                entity.ToTable("ArticleProducts");
            });

            modelBuilder.Entity<BlogComment>(entity =>
            {
                entity.HasKey(b => new { b.Id });

                entity.HasOne(d => d.BlogArticle)
                    .WithMany(c => c.BlogComments)
                    .HasForeignKey(d => d.BlogArticleId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ArticleComments_Articles");

                entity.Property(b => b.Id).HasColumnType("int").HasColumnName("blog_comment_id");
                entity.Property(b => b.BlogArticleId).HasColumnType("int").HasColumnName("blog_article_id");
                entity.Property(b => b.CustomerId).HasColumnType("int").HasColumnName("customer_id");
                entity.Property(b => b.Text).HasColumnType("nvarchar(2000)").HasColumnName("text");
                entity.Property(b => b.Posted).HasColumnType("datetime").HasColumnName("publication_date");

                entity.ToTable("ArticleComments");
            });
        }
    }
}
