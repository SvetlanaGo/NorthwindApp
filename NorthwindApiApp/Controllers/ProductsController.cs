using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Products;

namespace NorthwindApiApp.Controllers
{
    /// <summary>
    /// Class ProductController.
    /// </summary>
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductManagementService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsController"/> class.
        /// </summary>
        /// <param name="service">Product management service.</param>
        /// <exception cref="ArgumentNullException">Throw when service is null.</exception>
        public ProductsController(IProductManagementService service) =>
            this.service = service ?? throw new ArgumentNullException(nameof(service));

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="product">A <see cref="ProductModel"/> to create.</param>
        /// <returns>An identifier of a created product.</returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductModel>> CreateProductAsync(ProductModel product)
        {
            if (product is null)
            {
                return this.BadRequest();
            }

            var productId = await this.service.CreateProductAsync(product);

            return this.CreatedAtAction(nameof(this.ReadProductAsync), new { id = productId }, product);
        }

        /// <summary>
        /// Destroys an existed product.
        /// </summary>
        /// <param name="id">A product identifier.</param>
        /// <returns>True if a product is destroyed; otherwise false.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProductAsync([FromRoute] int id)
        {
            if (id <= 0)
            {
                return this.BadRequest();
            }

            var result = await this.service.DestroyProductAsync(id);

            return result ? this.NoContent() : this.NotFound();
        }

        /// <summary>
        /// Updates a product.
        /// </summary>
        /// <param name="id">A product identifier.</param>
        /// <param name="product">A <see cref="ProductModel"/>.</param>
        /// <returns>True if a product is updated; otherwise false.</returns>
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProductAsync([FromRoute] int id, ProductModel product)
        {
            if (product is null || id <= 0)
            {
                return this.BadRequest();
            }

            var result = await this.service.UpdateProductAsync(id, product);

            return result ? this.NoContent() : this.NotFound();
        }

        /// <summary>
        /// Show a product with specified identifier.
        /// </summary>
        /// <param name="id">A product identifier.</param>
        /// <returns>Returns true if a product is returned; otherwise false.</returns>
        [HttpGet("{id}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductModel>> ReadProductAsync([FromRoute] int id)
        {
            if (id < 0)
            {
                return this.BadRequest();
            }

            var product = await this.service.GetProductAsync(id);

            return product != null ? this.Ok(product) : this.NotFound();
        }

        /// <summary>
        /// Shows a list of product using specified offset and limit for pagination.
        /// </summary>
        /// <param name="offset">An offset of the first element to return.</param>
        /// <param name="limit">A limit of elements to return.</param>
        /// <returns>A <see cref="IEnumerable{T}"/> of <see cref="ProductModel"/>.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async IAsyncEnumerable<ProductModel> ReadProductsAsync([FromQuery] int offset = 0, [FromQuery] int limit = 10)
        {
            await foreach (var product in this.service.GetProductsAsync(offset, limit))
            {
                yield return product;
            }
        }

        /// <summary>
        /// Looks up for product with specified names.
        /// </summary>
        /// <param name="names">A list of product names.</param>
        /// <returns>A list of product with specified names.</returns>
        [HttpGet("by_names")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async IAsyncEnumerable<ProductModel> ReadProductsAsync([FromBody] IList<string> names)
        {
            if (names is null || names.Count < 1)
            {
                await Task.CompletedTask;
                yield break;
            }

            await foreach (var product in this.service.LookupProductsByNameAsync(names))
            {
                yield return product;
            }
        }

        /// <summary>
        /// Shows a list of products that belongs to a specified category.
        /// </summary>
        /// <param name="listOfCategoryId">A product category identifier.</param>
        /// <returns>A list of product with specified category identifiers.</returns>
        [HttpGet("by_categories_ids")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async IAsyncEnumerable<ProductModel> ReadProductsAsync([FromBody] IList<int> listOfCategoryId)
        {
            if (listOfCategoryId is null || listOfCategoryId.Count < 1)
            {
                await Task.CompletedTask;
                yield break;
            }

            await foreach (var product in this.service.GetProductsForCategoryAsync(listOfCategoryId))
            {
                yield return product;
            }
        }
    }
}
