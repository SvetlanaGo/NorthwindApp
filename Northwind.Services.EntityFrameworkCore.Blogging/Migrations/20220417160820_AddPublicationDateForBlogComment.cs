using System;
using Microsoft.EntityFrameworkCore.Migrations;

#pragma warning disable CA1062 // Validate arguments of public methods

namespace Northwind.Services.EntityFrameworkCore.Blogging.Migrations
{
    /// <summary>
    /// Class AddPublicationDateForBlogComment.
    /// </summary>
    public partial class AddPublicationDateForBlogComment : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "publication_date",
                table: "ArticleComments",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc/>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "publication_date",
                table: "ArticleComments");
        }
    }
}
