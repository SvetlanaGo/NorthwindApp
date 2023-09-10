﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Northwind.Services.EntityFrameworkCore.Blogging.Context;

namespace Northwind.Services.EntityFrameworkCore.Blogging.Migrations
{
    [DbContext(typeof(BloggingContext))]
    [Migration("20220411103101_AddFK_ArticleProducts_Products")]
    partial class AddFK_ArticleProducts_Products
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.14")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Northwind.Services.EntityFrameworkCore.Blogging.Entities.BlogArticle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("blog_article_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AuthorId")
                        .HasColumnType("int")
                        .HasColumnName("author_id");

                    b.Property<DateTime>("Posted")
                        .HasColumnType("datetime")
                        .HasColumnName("publication_date");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(2000)")
                        .HasColumnName("text");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("Northwind.Services.EntityFrameworkCore.Blogging.Entities.BlogArticleProduct", b =>
                {
                    b.Property<int>("BlogArticleId")
                        .HasColumnType("int")
                        .HasColumnName("blog_article_id");

                    b.Property<int>("ProductId")
                        .HasColumnType("int")
                        .HasColumnName("product_id");

                    b.HasKey("BlogArticleId", "ProductId");

                    b.ToTable("ArticleProducts");
                });

            modelBuilder.Entity("Northwind.Services.EntityFrameworkCore.Blogging.Entities.BlogArticleProduct", b =>
                {
                    b.HasOne("Northwind.Services.EntityFrameworkCore.Blogging.Entities.BlogArticle", "BlogArticle")
                        .WithMany("BlogArticleProducts")
                        .HasForeignKey("BlogArticleId")
                        .HasConstraintName("FK_ArticleProducts_Products")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BlogArticle");
                });

            modelBuilder.Entity("Northwind.Services.EntityFrameworkCore.Blogging.Entities.BlogArticle", b =>
                {
                    b.Navigation("BlogArticleProducts");
                });
#pragma warning restore 612, 618
        }
    }
}
