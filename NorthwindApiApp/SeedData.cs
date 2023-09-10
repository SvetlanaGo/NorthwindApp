using System;
using System.Linq;
using System.Net;
using Bogus;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.Services.Employees;
using Northwind.Services.EntityFrameworkCore.InMemory;
using Northwind.Services.Products;

namespace NorthwindApiApp
{
    /// <summary>
    /// Class SeedData.
    /// </summary>
    public static class SeedData
    {
        /// <summary>
        /// Generate seed data.
        /// </summary>
        /// <param name="app">ApplicationBuilder.</param>
        /// <param name="configuration">Configuration.</param>
        /// <exception cref="ArgumentNullException">Throw when context is null.</exception>
        public static void GenerateSeedData(IApplicationBuilder app, IConfiguration configuration)
        {
            if (app is null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            using var serviceScope = app.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<NorthwindContextInMemory>();

            string[] urls = configuration.GetSection("Urls").Get<string[]>();

            var itemCount = urls is null ? 0 : urls.Length;

            if (!context.ProductCategories.Any())
            {
                int id = 1;
                using var webClient = new WebClient();
                context.ProductCategories.AddRange(
                    new Faker<ProductCategoryModel>("en")
                    .RuleFor(x => x.Id, f => id++)
                    .RuleFor(x => x.Name, f => f.Commerce.Categories(1).First())
                    .RuleFor(x => x.Description, f => f.Commerce.ProductDescription())
                    .RuleFor(x => x.Picture, f => webClient.DownloadData(urls[f.Random.Number(0, itemCount - 1)]))
                    .Generate(itemCount));

                context.SaveChanges();
            }

            if (!context.Products.Any())
            {
                int id = 1;
                context.Products.AddRange(
                    new Faker<ProductModel>("en")
                    .RuleFor(x => x.Id, f => id++)
                    .RuleFor(x => x.Name, f => f.Commerce.ProductName())
                    .RuleFor(x => x.SupplierId, f => f.Random.Number(1, itemCount).OrNull(f, .2F))
                    .RuleFor(x => x.CategoryId, f => f.Random.Number(1, itemCount).OrNull(f, .2F))
                    .RuleFor(x => x.QuantityPerUnit, f => f.Lorem.Sentence(3))
                    .RuleFor(x => x.UnitPrice, f => f.Random.Decimal(0.0m).OrNull(f, .1F))
                    .RuleFor(x => x.UnitsInStock, f => f.Random.Short(0, 30000).OrNull(f, .15F))
                    .RuleFor(x => x.UnitsOnOrder, f => f.Random.Short(0, 30000).OrNull(f, .25F))
                    .RuleFor(x => x.ReorderLevel, f => f.Random.Short(0, 30).OrNull(f, .05F))
                    .RuleFor(x => x.Discontinued, f => f.Random.Bool())
                    .Generate(itemCount));

                context.SaveChanges();
            }

            if (!context.Employees.Any())
            {
                int id = 1, indexPhoto = 0, indexPath = 0;
                var fakeUrls = new int[itemCount];
                Random rnd = new Random();
                for (int i = 0; i < itemCount; i++)
                {
                    fakeUrls[i] = rnd.Next(0, 9);
                }

                using var webClient = new WebClient();
                context.Employees.AddRange(
                    new Faker<EmployeeModel>("en")
                    .RuleFor(x => x.Id, f => id++)
                    .RuleFor(x => x.LastName, f => f.Person.LastName)
                    .RuleFor(x => x.FirstName, f => f.Person.FirstName)
                    .RuleFor(x => x.Title, f => f.Name.JobTitle())
                    .RuleFor(x => x.TitleOfCourtesy, f => f.Name.Prefix(f.Person.Gender))
                    .RuleFor(x => x.BirthDate, f => f.Person.DateOfBirth.OrNull(f, .01F))
                    .RuleFor(x => x.HireDate, f => f.Date.Between(new DateTime(DateTime.Now.Year - 40, 01, 01), DateTime.Now))
                    .RuleFor(x => x.Address, f => f.Person.Address.Street)
                    .RuleFor(x => x.City, f => f.Person.Address.City)
                    .RuleFor(x => x.Region, f => "WA")
                    .RuleFor(x => x.PostalCode, f => f.Address.ZipCode())
                    .RuleFor(x => x.Country, f => f.Address.CountryCode())
                    .RuleFor(x => x.HomePhone, f => f.Phone.PhoneNumber())
                    .RuleFor(x => x.Extension, f => $"{f.Random.Short(0, 3000)}")
                    .RuleFor(x => x.Photo, f => webClient.DownloadData(urls[fakeUrls[indexPhoto++]]))
                    .RuleFor(x => x.Notes, f => f.Lorem.Sentence())
                    .RuleFor(x => x.ReportsTo, f => f.Random.Number(1, itemCount).OrNull(f, .1F))
                    .RuleFor(x => x.PhotoPath, f => urls[fakeUrls[indexPath++]])
                    .Generate(itemCount));

                context.SaveChanges();
            }
        }
    }
}
