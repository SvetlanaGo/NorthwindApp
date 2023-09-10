using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Northwind.Services.Blogging;
using Northwind.Services.EntityFrameworkCore.Blogging.Context;
using Northwind.Services.EntityFrameworkCore.Blogging.Entities;

namespace Northwind.Services.EntityFrameworkCore.Blogging
{
    /// <summary>
    /// Represents a blogging service.
    /// </summary>
    public sealed class BlogArticleProductService : IBlogArticleProductService
    {
        private readonly BloggingContext context;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogArticleProductService"/> class.
        /// </summary>
        /// <param name="factory">A DesignTimeDbContextFactory"/>.</param>
        /// <param name="mapper">A <see cref="IMapper"/>.</param>
        /// <exception cref="ArgumentNullException">Throw when factory or mapper is null.</exception>
        public BlogArticleProductService(IDesignTimeDbContextFactory<BloggingContext> factory, IMapper mapper)
        {
            if (factory is null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            this.context = factory.CreateDbContext(null);
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc/>
        public async Task<(int blogArticleId, int productId)> CreateBlogArticleProductAsync(BlogArticleProductModel blogArticleProduct)
        {
            TaskArgumentVerificator.CheckItemIsNull(blogArticleProduct);

            var blogArticleExists = await this.context.BlogArticles.AnyAsync(b => b.Id == blogArticleProduct.BlogArticleId);
            if (!blogArticleExists)
            {
                return (0, 0);
            }

            var blogArticleProductExists = await this.context.BlogArticleProducts.FindAsync(blogArticleProduct.BlogArticleId, blogArticleProduct.ProductId);
            if (blogArticleProductExists != null)
            {
                return (0, 0);
            }

            var transfer = this.mapper.Map<BlogArticleProduct>(blogArticleProduct);
            await this.context.BlogArticleProducts.AddAsync(transfer).ConfigureAwait(false);
            await this.context.SaveChangesAsync();

            return (transfer.BlogArticleId, transfer.ProductId);
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyBlogArticleProductAsync(BlogArticleProductModel blogArticleProduct)
        {
            TaskArgumentVerificator.CheckItemIsNull(blogArticleProduct);

            var transfer = await this.context.BlogArticleProducts.FindAsync(blogArticleProduct.BlogArticleId, blogArticleProduct.ProductId);
            if (transfer is null)
            {
                return false;
            }

            this.context.BlogArticleProducts.Remove(transfer);
            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        /// <inheritdoc/>
        public async Task<BlogArticleProductModel> GetBlogArticleProductAsync(int blogArticleId, int productId)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, blogArticleId, "Must be greater than zero.");
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, productId, "Must be greater than zero.");

            return this.mapper.Map<BlogArticleProductModel>(await this.context.BlogArticleProducts.FindAsync(blogArticleId, productId));
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<BlogArticleProductModel> GetBlogArticleProductsAsync(int blogArticleId)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, blogArticleId, "Must be greater than zero.");

            await foreach (var blogArticleProduct in this.context.BlogArticleProducts.Where(b => b.BlogArticleId == blogArticleId).AsAsyncEnumerable())
            {
                yield return this.mapper.Map<BlogArticleProductModel>(blogArticleProduct);
            }
        }
    }
}
