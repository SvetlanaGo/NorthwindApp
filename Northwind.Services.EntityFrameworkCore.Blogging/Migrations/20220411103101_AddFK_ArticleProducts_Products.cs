using Microsoft.EntityFrameworkCore.Migrations;

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA1062 // Validate arguments of public methods

namespace Northwind.Services.EntityFrameworkCore.Blogging.Migrations
{
    /// <summary>
    /// Class AddFK_ArticleProducts_Products.
    /// </summary>
    public partial class AddFK_ArticleProducts_Products : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_ArticleProducts_Products",
                table: "ArticleProducts",
                column: "blog_article_id",
                principalTable: "Articles",
                principalColumn: "blog_article_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc/>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleProducts_Products",
                table: "ArticleProducts");
        }
    }
}
