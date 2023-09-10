using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Threading.Tasks;
using Northwind.DataAccess.Products;

#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities

namespace Northwind.DataAccess.SqlServer.Products
{
    /// <summary>
    /// Represents a SQL Server-tailored DAO for Northwind product categories.
    /// </summary>
    public sealed class ProductCategorySqlServerDataAccessObject : IProductCategoryDataAccessObject
    {
        private readonly SqlConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategorySqlServerDataAccessObject"/> class.
        /// </summary>
        /// <param name="connection">A <see cref="SqlConnection"/>.</param>
        /// <exception cref="ArgumentNullException">Throw when connection is null.</exception>
        public ProductCategorySqlServerDataAccessObject(SqlConnection connection) =>
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));

        /// <inheritdoc/>
        public async Task<int> InsertProductCategoryAsync(ProductCategoryTransferObject productCategory)
        {
            TaskArgumentVerificator.CheckItemIsNull(productCategory);

            await using var command = new SqlCommand("InsertCategory", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            AddSqlParameters(productCategory, command);
            await this.connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();

            return (int)result;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteProductCategoryAsync(int productCategoryId)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, productCategoryId, "Must be greater than zero.");

            await using var command = new SqlCommand("DeleteCategory", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            const string categoryId = "@categoryID";
            command.Parameters.Add(categoryId, SqlDbType.Int);
            command.Parameters[categoryId].Value = productCategoryId;
            await this.connection.OpenAsync();
            var result = await command.ExecuteNonQueryAsync();

            return result > 0;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateProductCategoryAsync(ProductCategoryTransferObject productCategory)
        {
            TaskArgumentVerificator.CheckItemIsNull(productCategory);

            await using var command = new SqlCommand("UpdateCategory", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            AddSqlParameters(productCategory, command);

            const string categoryId = "@categoryId";
            command.Parameters.Add(categoryId, SqlDbType.Int);
            command.Parameters[categoryId].Value = productCategory.Id;
            await this.connection.OpenAsync();
            var result = await command.ExecuteNonQueryAsync();

            return result > 0;
        }

        /// <inheritdoc/>
        public async Task<ProductCategoryTransferObject> FindProductCategoryAsync(int productCategoryId)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, productCategoryId, "Must be greater than zero.");

            await using var command = new SqlCommand("FindCategory", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            const string categoryId = "@categoryId";
            command.Parameters.Add(categoryId, SqlDbType.Int);
            command.Parameters[categoryId].Value = productCategoryId;
            await this.connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
            {
                throw new ProductCategoryNotFoundException(productCategoryId);
            }

            return CreateProductCategory(reader);
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<ProductCategoryTransferObject> SelectProductCategoriesAsync(int offset, int limit)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 0, offset, "Must be greater than zero or equals zero.");
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 1, limit, "Must be greater than zero.");

            await using var command = new SqlCommand("SelectCategories", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            const string offsetCategories = "@offsetCategories";
            command.Parameters.Add(offsetCategories, SqlDbType.Int);
            command.Parameters[offsetCategories].Value = offset;

            const string limitCategories = "@limitCategories";
            command.Parameters.Add(limitCategories, SqlDbType.Int);
            command.Parameters[limitCategories].Value = limit;

            await this.connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                yield return CreateProductCategory(reader);
            }
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<ProductCategoryTransferObject> SelectProductCategoriesByNameAsync(ICollection<string> productCategoryNames)
        {
            TaskArgumentVerificator.CheckItemIsNull(productCategoryNames);
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 1, productCategoryNames.Count, "Collection is empty.");

            await using var stream = new MemoryStream();
            await new WriterToXml(stream).GetXmlAsync(productCategoryNames, "Names", "name");
            await using var command = new SqlCommand("SelectCategoriesByNames", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            const string productNamesParameter = "@categoryNames";
            command.Parameters.Add(productNamesParameter, SqlDbType.Xml);
            command.Parameters[productNamesParameter].Value = new SqlXml(stream);

            await this.connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                yield return CreateProductCategory(reader);
            }
        }

        private static ProductCategoryTransferObject CreateProductCategory(SqlDataReader reader)
        {
            var id = (int)reader["CategoryID"];
            var name = (string)reader["CategoryName"];

            const string descriptionColumnName = "Description";
            string description = reader[descriptionColumnName] != DBNull.Value ? (string)reader["Description"] : null;

            const string pictureColumnName = "Picture";
            byte[] picture = reader[pictureColumnName] != DBNull.Value ? (byte[])reader["Picture"] : null;

            return new ProductCategoryTransferObject
            {
                Id = id,
                Name = name,
                Description = description,
                Picture = picture,
            };
        }

        private static void AddSqlParameters(ProductCategoryTransferObject productCategory, SqlCommand command)
        {
            const string categoryNameParameter = "@categoryName";
            command.Parameters.Add(categoryNameParameter, SqlDbType.NVarChar, 15);
            command.Parameters[categoryNameParameter].Value = productCategory.Name;

            const string descriptionParameter = "@description";
            command.Parameters.Add(descriptionParameter, SqlDbType.NText);
            command.Parameters[descriptionParameter].IsNullable = true;
            command.Parameters[descriptionParameter].Value = productCategory.Description is null ? DBNull.Value : productCategory.Description;

            const string pictureParameter = "@picture";
            command.Parameters.Add(pictureParameter, SqlDbType.Image);
            command.Parameters[pictureParameter].IsNullable = true;
            command.Parameters[pictureParameter].Value = productCategory.Picture is null ? DBNull.Value : productCategory.Picture;
        }
    }
}
