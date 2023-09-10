using System;
using Microsoft.EntityFrameworkCore.Migrations;

#pragma warning disable CA1062 // Validate arguments of public methods

namespace Northwind.Services.EntityFrameworkCore.Blogging.Migrations
{
    /// <summary>
    /// Class InitialCreate.
    /// </summary>
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    blog_article_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    text = table.Column<string>(type: "nvarchar(2000)", nullable: true),
                    publication_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    author_id = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.blog_article_id);
                });
        }

        /// <inheritdoc/>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");
        }
    }
}
