using Microsoft.EntityFrameworkCore.Migrations;

#pragma warning disable CA1062 // Validate arguments of public methods

namespace Northwind.Services.EntityFrameworkCore.Blogging.Migrations
{
    /// <summary>
    /// Class AddBlogArticleProduct.
    /// </summary>
    public partial class AddBlogArticleProduct : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticleProducts",
                columns: table => new
                {
                    blog_article_id = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleProducts", x => new { x.blog_article_id, x.product_id });
                });
        }

        /// <inheritdoc/>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleProducts");
        }
    }
}
