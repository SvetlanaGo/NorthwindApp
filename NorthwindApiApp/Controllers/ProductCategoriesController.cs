using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Products;

namespace NorthwindApiApp.Controllers
{
    /// <summary>
    /// Class ProductCategoriesController.
    /// </summary>
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]")]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IProductCategoryManagementService service;
        private readonly IProductCategoryPicturesManagementService pictureService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoriesController"/> class.
        /// </summary>
        /// <param name="service">Product category management service.</param>
        /// <param name="pictureService">Product category picture management service.</param>
        /// <exception cref="ArgumentNullException">Throw when product category management service or product category picture management service is null.</exception>
        public ProductCategoriesController(IProductCategoryManagementService service, IProductCategoryPicturesManagementService pictureService)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.pictureService = pictureService ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// Creates a new product category.
        /// </summary>
        /// <param name="productCategory">A <see cref="ProductCategoryModel"/> to create.</param>
        /// <returns>An identifier of a created product category.</returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductCategoryModel>> CreateCategoryAsync([FromBody] ProductCategoryModel productCategory)
        {
            if (productCategory is null)
            {
                return this.BadRequest();
            }

            var productCategoryId = await this.service.CreateCategoryAsync(productCategory);

            return this.CreatedAtAction(nameof(this.ReadCategoryAsync), new { id = productCategoryId }, productCategory);
        }

        /// <summary>
        /// Destroys an existed product category.
        /// </summary>
        /// <param name="id">A product category identifier.</param>
        /// <returns>True if a product category is destroyed; otherwise false.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategoryAsync([FromRoute] int id)
        {
            if (id <= 0)
            {
                return this.BadRequest();
            }

            var result = await this.service.DestroyCategoryAsync(id);

            return result ? this.NoContent() : this.NotFound();
        }

        /// <summary>
        /// Updates a product category.
        /// </summary>
        /// <param name="id">A product category identifier.</param>
        /// <param name="productCategory">A <see cref="ProductCategoryModel"/>.</param>
        /// <returns>True if a product category is updated; otherwise false.</returns>
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategoryAsync([FromRoute] int id, ProductCategoryModel productCategory)
        {
            if (productCategory is null || id <= 0)
            {
                return this.BadRequest();
            }

            var result = await this.service.UpdateCategoriesAsync(id, productCategory);

            return result ? this.NoContent() : this.NotFound();
        }

        /// <summary>
        /// Show a product category with specified identifier.
        /// </summary>
        /// <param name="id">A product category identifier.</param>
        /// <returns>Returns true if a product category is returned; otherwise false.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductCategoryModel>> ReadCategoryAsync([FromRoute] int id)
        {
            if (id < 0)
            {
                return this.BadRequest();
            }

            var productCategory = await this.service.GetCategoryAsync(id);

            return productCategory != null ? this.Ok(productCategory) : this.NotFound();
        }

        /// <summary>
        /// Shows a list of product categories using specified offset and limit for pagination.
        /// </summary>
        /// <param name="offset">An offset of the first element to return.</param>
        /// <param name="limit">A limit of elements to return.</param>
        /// <returns>A <see cref="IEnumerable{T}"/> of <see cref="ProductCategoryModel"/>.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async IAsyncEnumerable<ProductCategoryModel> ReadCategoriesAsync([FromQuery] int offset = 0, [FromQuery] int limit = 10)
        {
            await foreach (var productCategory in this.service.GetCategoriesAsync(offset, limit))
            {
                yield return productCategory;
            }
        }

        /// <summary>
        /// Looks up for product categories with specified names.
        /// </summary>
        /// <param name="names">A list of product category names.</param>
        /// <returns>A list of product categories with specified names.</returns>
        [HttpGet("by_names")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async IAsyncEnumerable<ProductCategoryModel> ReadCategoriesAsync([FromBody] IList<string> names)
        {
            if (names is null || names.Count < 1)
            {
                await Task.CompletedTask;
                yield break;
            }

            await foreach (var productCategory in this.service.LookupCategoriesByNameAsync(names))
            {
                yield return productCategory;
            }
        }

        /// <summary>
        /// Update a product category picture.
        /// </summary>
        /// <param name="id">A product category identifier.</param>
        /// <param name="formFile">A <see cref="IFormFile"/>.</param>
        /// <returns>True if a product category picture is updated; otherwise false.</returns>
        [HttpPut("{id}/picture")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UploadPictureAsync([FromRoute] int id, IFormFile formFile)
        {
            if (formFile is null || id <= 0)
            {
                return this.BadRequest();
            }

            using var stream = new MemoryStream();
            await formFile.CopyToAsync(stream);
            var result = await this.pictureService.UpdatePictureAsync(id, stream);

            return result ? this.NoContent() : this.NotFound();
        }

        /// <summary>
        /// Destroys an existed product category picture.
        /// </summary>
        /// <param name="id">A product category identifier.</param>
        /// <returns>True if a product category picture is destroyed; otherwise false.</returns>
        [HttpDelete("{id}/picture")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePictureAsync([FromRoute] int id)
        {
            if (id <= 0)
            {
                return this.BadRequest();
            }

            var result = await this.pictureService.DestroyPictureAsync(id);

            return result ? this.NoContent() : this.NotFound();
        }

        /// <summary>
        /// Show a product category picture with specified identifier.
        /// </summary>
        /// <param name="id">A product category identifier.</param>
        /// <returns>Returns true if a product category picture is returned; otherwise false.</returns>
        [HttpGet("{id}/picture")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<byte[]>> GetPictureAsync([FromRoute] int id)
        {
            if (id < 0)
            {
                return this.BadRequest();
            }

            var picture = await this.pictureService.GetPictureAsync(id);

            return picture.Length != 0 ? this.File(picture, "image/bmp") : this.NotFound();
        }
    }
}
