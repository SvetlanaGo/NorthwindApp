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
    public sealed class BlogCommentService : IBlogCommentService
    {
        private readonly BloggingContext context;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogCommentService"/> class.
        /// </summary>
        /// <param name="factory">A DesignTimeDbContextFactory.</param>
        /// <param name="mapper">A <see cref="IMapper"/>.</param>
        /// <exception cref="ArgumentNullException">Throw when context or mapper is null.</exception>
        public BlogCommentService(IDesignTimeDbContextFactory<BloggingContext> factory, IMapper mapper)
        {
            if (factory is null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.context = factory.CreateDbContext(null);
        }

        /// <inheritdoc/>
        public async Task<int> CreateBlogCommentAsync(int blogArticleId, BlogCommentModel blogComment)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, blogArticleId, "Must be greater than zero.");
            TaskArgumentVerificator.CheckItemIsNull(blogComment);

            if (blogArticleId != blogComment.BlogArticleId)
            {
                return 0;
            }

            var blogArticleExists = await this.context.BlogArticles.AnyAsync(b => b.Id == blogArticleId);
            if (!blogArticleExists)
            {
                return 0;
            }

            var transfer = this.mapper.Map<BlogComment>(blogComment);
            await this.context.BlogComments.AddAsync(transfer).ConfigureAwait(false);
            await this.context.SaveChangesAsync();

            return transfer.Id;
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyBlogCommentAsync(int blogArticleId, int blogCommentId)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, blogArticleId, "Must be greater than zero.");
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, blogCommentId, "Must be greater than zero.");

            var blogComment = await this.context.BlogComments.FindAsync(blogCommentId);
            if (blogComment is null || blogArticleId != blogComment.BlogArticleId)
            {
                return false;
            }

            this.context.BlogComments.Remove(blogComment);
            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateBlogCommentAsync(int blogCommentId, BlogCommentModel blogComment)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, blogCommentId, "Must be greater than zero.");
            TaskArgumentVerificator.CheckItemIsNull(blogComment);

            var blogCommentUp = await this.context.BlogComments.FindAsync(blogCommentId);
            if (blogCommentUp is null)
            {
                return false;
            }

            blogCommentUp.BlogArticleId = blogComment.BlogArticleId;
            blogCommentUp.CustomerId = blogComment.CustomerId;
            blogCommentUp.Text = blogComment.Text;
            blogCommentUp.Posted = blogComment.Posted;

            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        /// <inheritdoc/>
        public async Task<BlogCommentModel> GetBlogCommentAsync(int blogArticleId, int blogCommentId)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, blogArticleId, "Must be greater than zero.");
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, blogCommentId, "Must be greater than zero.");

            return this.mapper.Map<BlogCommentModel>(await this.context.BlogComments.Where(b => b.Id == blogCommentId && b.BlogArticleId == blogArticleId).FirstOrDefaultAsync());
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<BlogCommentModel> GetBlogCommentsAsync(int blogArticleId, int offset, int limit)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, blogArticleId, "Must be greater than zero.");
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 0, offset, "Must be greater than zero or equals zero.");
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 1, limit, "Must be greater than zero.");

            await foreach (var blogComment in this.context.BlogComments.Where(b => b.BlogArticleId == blogArticleId).OrderBy(m => m.Id).Skip(offset).Take(limit).AsAsyncEnumerable())
            {
                yield return this.mapper.Map<BlogCommentModel>(blogComment);
            }
        }
    }
}
