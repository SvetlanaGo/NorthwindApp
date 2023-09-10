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
    public sealed class BloggingService : IBloggingService
    {
        private readonly BloggingContext context;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="BloggingService"/> class.
        /// </summary>
        /// <param name="factory">A DesignTimeDbContextFactory.</param>
        /// <param name="mapper">A <see cref="IMapper"/>.</param>
        /// <exception cref="ArgumentNullException">Throw when context or mapper is null.</exception>
        public BloggingService(IDesignTimeDbContextFactory<BloggingContext> factory, IMapper mapper)
        {
            if (factory is null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.context = factory.CreateDbContext(null);
        }

        /// <inheritdoc/>
        public async Task<int> CreateBlogArticleAsync(BlogArticleModel blogArticle)
        {
            TaskArgumentVerificator.CheckItemIsNull(blogArticle);

            var transfer = this.mapper.Map<BlogArticle>(blogArticle);
            await this.context.BlogArticles.AddAsync(transfer).ConfigureAwait(false);
            await this.context.SaveChangesAsync();

            return transfer.Id;
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyBlogArticleAsync(int blogArticleId)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, blogArticleId, "Must be greater than zero.");

            var blogArticle = await this.context.BlogArticles.FindAsync(blogArticleId);
            if (blogArticle is null)
            {
                return false;
            }

            this.context.BlogArticles.Remove(blogArticle);
            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateBlogArticleAsync(int blogArticleId, BlogArticleModel blogArticle)
        {
            TaskArgumentVerificator.CheckItemIsNull(blogArticle);
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, blogArticleId, "Must be greater than zero.");

            var blogArticleUp = await this.context.BlogArticles.FindAsync(blogArticleId);
            if (blogArticleUp is null)
            {
                return false;
            }

            blogArticleUp.Title = blogArticle.Title;
            blogArticleUp.Text = blogArticle.Text;
            blogArticleUp.Posted = blogArticle.Posted;
            blogArticleUp.AuthorId = blogArticle.AuthorId;

            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        /// <inheritdoc/>
        public async Task<BlogArticleModel> GetBlogArticleAsync(int blogArticleId)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, blogArticleId, "Must be greater than zero.");

            return this.mapper.Map<BlogArticleModel>(await this.context.BlogArticles.FindAsync(blogArticleId));
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<BlogArticleModel> GetBlogArticlesAsync(int offset, int limit)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 0, offset, "Must be greater than zero or equals zero.");
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 1, limit, "Must be greater than zero.");

            await foreach (var blogArticle in this.context.BlogArticles.OrderBy(m => m.Id).Skip(offset).Take(limit).AsAsyncEnumerable())
            {
                yield return this.mapper.Map<BlogArticleModel>(blogArticle);
            }
        }
    }
}
