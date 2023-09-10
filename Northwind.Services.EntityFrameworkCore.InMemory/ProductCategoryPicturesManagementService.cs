using System;
using System.IO;
using System.Threading.Tasks;
using Northwind.Services.Products;

namespace Northwind.Services.EntityFrameworkCore.InMemory
{
    /// <summary>
    /// Represents a stub for a product management service.
    /// </summary>
    public sealed class ProductCategoryPicturesManagementService : IProductCategoryPicturesManagementService
    {
        private readonly NorthwindContextInMemory context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoryPicturesManagementService"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <exception cref="ArgumentNullException">Throw when context is null.</exception>
        public ProductCategoryPicturesManagementService(NorthwindContextInMemory context) =>
            this.context = context ?? throw new ArgumentNullException(nameof(context));

        /// <inheritdoc/>
        public async Task<bool> DestroyPictureAsync(int categoryId)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, categoryId, "Must be greater than zero.");

            var category = await this.context.ProductCategories.FindAsync(categoryId);
            if (category is null)
            {
                return false;
            }

            category.Picture = null;
            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdatePictureAsync(int categoryId, Stream stream)
        {
            TaskArgumentVerificator.CheckItemIsNull(stream);
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, categoryId, "Must be greater than zero.");

            var category = await this.context.ProductCategories.FindAsync(categoryId);
            if (category is null)
            {
                return false;
            }

            var picture = new byte[stream.Length];
            await using var memoryStream = new MemoryStream(picture);
            stream.Seek(0, SeekOrigin.Begin);
            await stream.CopyToAsync(memoryStream);
            await stream.FlushAsync();
            category.Picture = picture;
            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        /// <inheritdoc/>
        public async Task<byte[]> GetPictureAsync(int categoryId)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, categoryId, "Must be greater than zero.");

            var category = await this.context.ProductCategories.FindAsync(categoryId);
            if (category is null || category.Picture is null)
            {
                return Array.Empty<byte>();
            }

            return category.Picture;
        }
    }
}
