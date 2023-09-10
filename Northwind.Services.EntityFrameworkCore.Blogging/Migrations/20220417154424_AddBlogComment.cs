using Microsoft.EntityFrameworkCore.Migrations;

#pragma warning disable CA1062 // Validate arguments of public methods

namespace Northwind.Services.EntityFrameworkCore.Blogging.Migrations
{
    /// <summary>
    /// Class AddBlogComment.
    /// </summary>
    public partial class AddBlogComment : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleProducts_Products",
                table: "ArticleProducts");

            migrationBuilder.CreateTable(
                name: "ArticleComments",
                columns: table => new
                {
                    blog_comment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    blog_article_id = table.Column<int>(type: "int", nullable: false),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    text = table.Column<string>(type: "nvarchar(2000)", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleComments", x => x.blog_comment_id);
                    table.ForeignKey(
                        name: "FK_ArticleComments_Articles",
                        column: x => x.blog_article_id,
                        principalTable: "Articles",
                        principalColumn: "blog_article_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleComments_blog_article_id",
                table: "ArticleComments",
                column: "blog_article_id");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleProducts_Articles",
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
                name: "FK_ArticleProducts_Articles",
                table: "ArticleProducts");

            migrationBuilder.DropTable(
                name: "ArticleComments");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleProducts_Products",
                table: "ArticleProducts",
                column: "blog_article_id",
                principalTable: "Articles",
                principalColumn: "blog_article_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
