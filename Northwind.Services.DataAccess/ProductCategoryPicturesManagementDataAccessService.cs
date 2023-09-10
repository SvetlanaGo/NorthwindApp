using System;
using System.IO;
using System.Threading.Tasks;
using Northwind.DataAccess;
using Northwind.DataAccess.Products;
using Northwind.Services.Products;

namespace Northwind.Services.DataAccess
{
    /// <summary>
    /// Represents a management service for product category pictures.
    /// </summary>
    public sealed class ProductCategoryPicturesManagementDataAccessService : IProductCategoryPicturesManagementService
    {
        private const int OleHeader = 78;
        private readonly NorthwindDataAccessFactory accessFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoryPicturesManagementDataAccessService"/> class.
        /// </summary>
        /// <param name="accessFactory">A <see cref="NorthwindDataAccessFactory"/>.</param>
        /// <exception cref="ArgumentNullException">Throw when accessFactory or mapper is null.</exception>
        public ProductCategoryPicturesManagementDataAccessService(NorthwindDataAccessFactory accessFactory) =>
            this.accessFactory = accessFactory ?? throw new ArgumentNullException(nameof(accessFactory));

        /// <inheritdoc/>
        public async Task<bool> DestroyPictureAsync(int categoryId)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, categoryId, "Must be greater than zero.");

            try
            {
                var category = this.accessFactory.GetProductCategoryDataAccessObject();
                var transfer = await category.FindProductCategoryAsync(categoryId);
                transfer.Picture = null;
                var result = await category.UpdateProductCategoryAsync(transfer);

                return result;
            }
            catch (ProductCategoryNotFoundException)
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> UpdatePictureAsync(int categoryId, Stream stream)
        {
            TaskArgumentVerificator.CheckItemIsNull(stream);
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, categoryId, "Must be greater than zero.");

            try
            {
                var category = this.accessFactory.GetProductCategoryDataAccessObject();
                var transfer = await category.FindProductCategoryAsync(categoryId);
                var picture = new byte[stream.Length + OleHeader];
                await using var memoryStream = new MemoryStream(picture);
                stream.Seek(0, SeekOrigin.Begin);
                memoryStream.Seek(OleHeader, SeekOrigin.Begin);
                await stream.CopyToAsync(memoryStream);
                await stream.FlushAsync();
                transfer.Picture = picture;
                var result = await category.UpdateProductCategoryAsync(transfer);

                return result;
            }
            catch (ProductCategoryNotFoundException)
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<byte[]> GetPictureAsync(int categoryId)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, categoryId, "Must be greater than zero.");

            try
            {
                var transfer = await this.accessFactory
                    .GetProductCategoryDataAccessObject()
                    .FindProductCategoryAsync(categoryId);

                return transfer.Picture is null ? Array.Empty<byte>() : transfer.Picture[OleHeader..];
            }
            catch (ProductCategoryNotFoundException)
            {
                return Array.Empty<byte>();
            }
        }
    }
}
