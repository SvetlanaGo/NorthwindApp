using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Blogging;
using Northwind.Services.Employees;
using Northwind.Services.Products;
using NorthwindApiApp.Models;

namespace NorthwindApiApp.Controllers
{
    /// <summary>
    /// Class BlogArticlesController.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BlogArticlesController : ControllerBase
    {
        private readonly IBloggingService service;
        private readonly IBlogArticleProductService blogArticleProductService;
        private readonly IBlogCommentService blogCommentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogArticlesController"/> class.
        /// </summary>
        /// <param name="service">Blog articles service.</param>
        /// <param name="blogArticleProductService">Blog articles product service.</param>
        /// <param name="blogCommentService">Blog comment service.</param>
        /// <exception cref="ArgumentNullException">Throw when blog articles service or blog articles product service or blogCommentService is null.</exception>
        public BlogArticlesController(IBloggingService service, IBlogArticleProductService blogArticleProductService, IBlogCommentService blogCommentService)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.blogArticleProductService = blogArticleProductService ?? throw new ArgumentNullException(nameof(blogArticleProductService));
            this.blogCommentService = blogCommentService ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// Creates a new blog article.
        /// </summary>
        /// <param name="target">A <see cref="BlogArticleCreatedBindingTarget"/> to create.</param>
        /// <param name="employeeService">Employee management service.</param>
        /// <returns>An identifier of a created blog article.</returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BlogArticleCreatedBindingTarget>> CreateBlogArticleAsync(BlogArticleCreatedBindingTarget target, [FromServices] IEmployeeManagementService employeeService)
        {
            if (target is null || target.AuthorId <= 0 || employeeService is null)
            {
                return this.BadRequest();
            }

            var employee = await employeeService.GetEmployeeAsync(target.AuthorId);
            if (employee is null)
            {
                return this.BadRequest();
            }

            var blogArticleId = await this.service.CreateBlogArticleAsync(target.ToBlogArticle());

            return this.CreatedAtAction(nameof(this.ReadBlogArticleAsync), new { id = blogArticleId }, target);
        }

        /// <summary>
        /// Destroys an existed blog article.
        /// </summary>
        /// <param name="id">A blog article identifier.</param>
        /// <returns>True if an blog article is destroyed; otherwise false.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBlogArticleAsync([FromRoute] int id)
        {
            if (id <= 0)
            {
                return this.BadRequest();
            }

            var result = await this.service.DestroyBlogArticleAsync(id);

            return result ? this.NoContent() : this.NotFound();
        }

        /// <summary>
        /// Updates a blog article.
        /// </summary>
        /// <param name="id">A blog article identifier.</param>
        /// <param name="target">A <see cref="BlogArticleUpdatedBindingTarget"/>.</param>
        /// <returns>True if an blog article is updated; otherwise false.</returns>
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBlogArticleAsync([FromRoute] int id, BlogArticleUpdatedBindingTarget target)
        {
            if (target is null || id <= 0)
            {
                return this.BadRequest();
            }

            var blogArticle = await this.service.GetBlogArticleAsync(id);
            if (blogArticle is null)
            {
                return this.NotFound();
            }

            var result = await this.service.UpdateBlogArticleAsync(id, target.ToBlogArticle(blogArticle.AuthorId));

            return result ? this.NoContent() : this.NotFound();
        }

        /// <summary>
        /// Show a blog article with specified identifier.
        /// </summary>
        /// <param name="employeeService">Employee management service.</param>
        /// <param name="id">A blog article identifier.</param>
        /// <returns>Returns true if a blog article is returned; otherwise false.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BlogArticleFullReadedBindingTarget>> ReadBlogArticleAsync([FromServices] IEmployeeManagementService employeeService, [FromRoute] int id)
        {
            if (employeeService is null || id <= 0)
            {
                return this.BadRequest();
            }

            var blogArticle = await this.service.GetBlogArticleAsync(id);
            if (blogArticle is null)
            {
                return this.NotFound();
            }

            var employee = await employeeService.GetEmployeeAsync(blogArticle.AuthorId);
            var target = new BlogArticleFullReadedBindingTarget(blogArticle, $"{employee.FirstName} {employee.LastName}, {employee.Title}");

            return this.Ok(target);
        }

        /// <summary>
        /// Shows a list of blog article using specified offset and limit for pagination.
        /// </summary>
        /// <param name="employeeService">Employee management service.</param>
        /// <param name="offset">An offset of the first element to return.</param>
        /// <param name="limit">A limit of elements to return.</param>
        /// <returns>A <see cref="IEnumerable{T}"/> of <see cref="BlogArticleReadedBindingTarget"/>.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async IAsyncEnumerable<BlogArticleReadedBindingTarget> ReadBlogArticlesAsync([FromServices] IEmployeeManagementService employeeService, [FromQuery] int offset = 0, [FromQuery] int limit = 10)
        {
            if (employeeService is null)
            {
                await Task.CompletedTask;
                yield break;
            }

            await foreach (var blogArticle in this.service.GetBlogArticlesAsync(offset, limit))
            {
                var employee = await employeeService.GetEmployeeAsync(blogArticle.AuthorId);
                yield return new BlogArticleReadedBindingTarget(blogArticle, $"{employee.FirstName} {employee.LastName}, {employee.Title}");
            }
        }

        /// <summary>
        /// Creates a new blog article product.
        /// </summary>
        /// <param name="articleId">A blog article identifier.</param>
        /// <param name="id">A product identifier.</param>
        /// <param name="productService">Product management service.</param>
        /// <returns>True if an blog article product is created; otherwise false.</returns>
        [HttpPost("{articleId}/products/{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BlogArticleProductModel>> CreateBlogArticleProductAsync([FromRoute] int articleId, [FromRoute] int id, [FromServices] IProductManagementService productService)
        {
            if (articleId <= 0 || id <= 0 || productService is null)
            {
                return this.BadRequest();
            }

            var product = await productService.GetProductAsync(id);
            if (product is null)
            {
                return this.BadRequest();
            }

            var blogArticleProduct =
                new BlogArticleProductModel()
                {
                    BlogArticleId = articleId,
                    ProductId = id,
                };

            var blogArticleProductId = await this.blogArticleProductService.CreateBlogArticleProductAsync(blogArticleProduct);
            if (blogArticleProductId.blogArticleId < 1)
            {
                return this.BadRequest();
            }

            return this.CreatedAtAction(nameof(this.ReadBlogArticleProductAsync), new { articleId = blogArticleProductId.blogArticleId, id = blogArticleProductId.productId }, blogArticleProduct);
        }

        /// <summary>
        /// Destroys an existed blog article product.
        /// </summary>
        /// <param name="articleId">A blog article identifier.</param>
        /// <param name="id">A product identifier.</param>
        /// <returns>True if an blog article is destroyed; otherwise false.</returns>
        [HttpDelete("{articleId}/products/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBlogArticleProductAsync([FromRoute] int articleId, [FromRoute] int id)
        {
            if (articleId <= 0 || id <= 0)
            {
                return this.BadRequest();
            }

            var result = await this.blogArticleProductService.DestroyBlogArticleProductAsync(
                new BlogArticleProductModel()
                {
                    BlogArticleId = articleId,
                    ProductId = id,
                });

            return result ? this.NoContent() : this.NotFound();
        }

        /// <summary>
        /// Show a related product with specified identifier.
        /// </summary>
        /// <param name="articleId">A blog article identifier.</param>
        /// <param name="id">A product identifier.</param>
        /// <returns>Returns true if a product category is returned; otherwise false.</returns>
        [HttpGet("{articleId}/products/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> ReadBlogArticleProductAsync([FromRoute] int articleId, [FromRoute] int id)
        {
            if (articleId <= 0 || id <= 0)
            {
                return this.BadRequest();
            }

            var blogArticleProduct = await this.blogArticleProductService.GetBlogArticleProductAsync(articleId, id);

            return blogArticleProduct != null ? this.Ok(this.Url.RouteUrl(new { controller = "Products", action = "ReadProductAsync", id = blogArticleProduct.ProductId })) : this.NotFound();
        }

        /// <summary>
        /// Shows a list of all related products.
        /// </summary>
        /// <param name="articleId">A blog article identifier.</param>
        /// <returns>A list of all related products/>.</returns>
        [HttpGet("{articleId}/products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async IAsyncEnumerable<Uri> ReadBlogArticleProductsAsync([FromRoute] int articleId)
        {
            if (articleId <= 0)
            {
                await Task.CompletedTask;
                yield break;
            }

            await foreach (var blogArticleProduct in this.blogArticleProductService.GetBlogArticleProductsAsync(articleId))
            {
                yield return new Uri(this.Url.Link("GetProduct", new { id = blogArticleProduct.ProductId }));
            }
        }

        /// <summary>
        /// Creates a new blog comment.
        /// </summary>
        /// <param name="articleId">A blog article identifier.</param>
        /// <param name="target">A <see cref="BlogCommentCreatedBindingTarget"/> to create.</param>
        /// <returns>An identifier of a created blog comment.</returns>
        [HttpPost("{articleId}/comments")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BlogCommentCreatedBindingTarget>> CreateBlogCommentAsync([FromRoute] int articleId, BlogCommentCreatedBindingTarget target)
        {
            if (articleId <= 0 || target is null || target.CustomerId <= 0)
            {
                return this.BadRequest();
            }

            var blogCommentId = await this.blogCommentService.CreateBlogCommentAsync(articleId, target.ToBlogArticle(articleId));

            return blogCommentId <= 0 ? this.BadRequest() : this.CreatedAtAction(nameof(this.ReadBlogCommentAsync), new { articleId = articleId, id = blogCommentId }, target);
        }

        /// <summary>
        /// Destroys an existed blog comment.
        /// </summary>
        /// <param name="articleId">A blog article identifier.</param>
        /// <param name="id">A blog comment identifier.</param>
        /// <returns>True if an blog comment is destroyed; otherwise false.</returns>
        [HttpDelete("{articleId}/comments/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBlogCommentAsync([FromRoute] int articleId, [FromRoute] int id)
        {
            if (articleId <= 0 || id <= 0)
            {
                return this.BadRequest();
            }

            var result = await this.blogCommentService.DestroyBlogCommentAsync(articleId, id);

            return result ? this.NoContent() : this.NotFound();
        }

        /// <summary>
        /// Updates a blog comment.
        /// </summary>
        /// <param name="articleId">A blog article identifier.</param>
        /// <param name="id">A blog comment identifier.</param>
        /// <param name="target">A <see cref="BlogCommentUpdatedBindingTarget"/>.</param>
        /// <returns>True if an blog comment is updated; otherwise false.</returns>
        [HttpPut("{articleId}/comments/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBlogCommentAsync([FromRoute] int articleId, [FromRoute] int id, BlogCommentUpdatedBindingTarget target)
        {
            if (articleId <= 0 || id <= 0 || target is null)
            {
                return this.BadRequest();
            }

            var blogComment = await this.blogCommentService.GetBlogCommentAsync(articleId, id);
            if (blogComment is null)
            {
                return this.NotFound();
            }

            var result = await this.blogCommentService.UpdateBlogCommentAsync(id, target.ToBlogComment(blogComment.BlogArticleId, blogComment.CustomerId));

            return result ? this.NoContent() : this.NotFound();
        }

        /// <summary>
        /// Show a blog comment with specified identifier.
        /// </summary>
        /// <param name="articleId">A blog article identifier.</param>
        /// <param name="id">A blog comment identifier.</param>
        /// <returns>Returns true if a blog comment is returned; otherwise false.</returns>
        [HttpGet("{articleId}/comments/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BlogCommentReadedBindingTarget>> ReadBlogCommentAsync([FromRoute] int articleId, [FromRoute] int id)
        {
            if (articleId <= 0 || id <= 0)
            {
                return this.BadRequest();
            }

            var blogComment = await this.blogCommentService.GetBlogCommentAsync(articleId, id);

            return blogComment != null ? this.Ok(new BlogCommentReadedBindingTarget(blogComment)) : this.NotFound();
        }

        /// <summary>
        /// Shows a list of blog comment using specified offset and limit for pagination.
        /// </summary>
        /// <param name="articleId">A blog article identifier.</param>
        /// <param name="offset">An offset of the first element to return.</param>
        /// <param name="limit">A limit of elements to return.</param>
        /// <returns>A <see cref="IEnumerable{T}"/> of <see cref="BlogCommentReadedBindingTarget"/>.</returns>
        [HttpGet("{articleId}/comments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async IAsyncEnumerable<BlogCommentReadedBindingTarget> ReadBlogCommentsAsync([FromRoute] int articleId, [FromQuery] int offset = 0, [FromQuery] int limit = 10)
        {
            if (articleId <= 0)
            {
                await Task.CompletedTask;
                yield break;
            }

            await foreach (var blogComment in this.blogCommentService.GetBlogCommentsAsync(articleId, offset, limit))
            {
                yield return new BlogCommentReadedBindingTarget(blogComment);
            }
        }
    }
}
