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
    /// Represents a SQL Server-tailored DAO for Northwind products.
    /// </summary>
    public sealed class ProductSqlServerDataAccessObject : IProductDataAccessObject
    {
        private readonly SqlConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductSqlServerDataAccessObject"/> class.
        /// </summary>
        /// <param name="connection">A <see cref="SqlConnection"/>.</param>
        /// <exception cref="ArgumentNullException">Throw when connection is null.</exception>
        public ProductSqlServerDataAccessObject(SqlConnection connection) =>
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));

        /// <inheritdoc/>
        public async Task<int> InsertProductAsync(ProductTransferObject product)
        {
            TaskArgumentVerificator.CheckItemIsNull(product);

            await using var command = new SqlCommand("InsertProduct", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            AddSqlParameters(product, command);
            await this.connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();

            return (int)result;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteProductAsync(int productId)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, productId, "Must be greater than zero.");

            await using var command = new SqlCommand("DeleteProduct", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            const string productIdParameter = "@productID";
            command.Parameters.Add(productIdParameter, SqlDbType.Int);
            command.Parameters[productIdParameter].Value = productId;
            await this.connection.OpenAsync();
            var result = await command.ExecuteNonQueryAsync();

            return result > 0;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateProductAsync(ProductTransferObject product)
        {
            TaskArgumentVerificator.CheckItemIsNull(product);

            await using var command = new SqlCommand("UpdateProduct", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            AddSqlParameters(product, command);

            const string productId = "@productId";
            command.Parameters.Add(productId, SqlDbType.Int);
            command.Parameters[productId].Value = product.Id;
            await this.connection.OpenAsync();
            var result = await command.ExecuteNonQueryAsync();

            return result > 0;
        }

        /// <inheritdoc/>
        public async Task<ProductTransferObject> FindProductAsync(int productId)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, productId, "Must be greater than zero.");

            await using var command = new SqlCommand("FindProduct", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            const string productIdParameter = "@productId";
            command.Parameters.Add(productIdParameter, SqlDbType.Int);
            command.Parameters[productIdParameter].Value = productId;
            await this.connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
            {
                throw new ProductNotFoundException(productId);
            }

            return CreateProduct(reader);
        }

        /// <inheritdoc />
        public async IAsyncEnumerable<ProductTransferObject> SelectProductsAsync(int offset, int limit)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 0, offset, "Must be greater than zero or equals zero.");
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 1, limit, "Must be greater than zero.");

            await using var command = new SqlCommand("SelectProducts", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            const string offsetProducts = "@offsetProducts";
            command.Parameters.Add(offsetProducts, SqlDbType.Int);
            command.Parameters[offsetProducts].Value = offset;

            const string limitProducts = "@limitProducts";
            command.Parameters.Add(limitProducts, SqlDbType.Int);
            command.Parameters[limitProducts].Value = limit;

            await this.connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                yield return CreateProduct(reader);
            }
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<ProductTransferObject> SelectProductsByNameAsync(ICollection<string> productNames)
        {
            TaskArgumentVerificator.CheckItemIsNull(productNames);
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 1, productNames.Count, "Collection is empty.");

            await using var stream = new MemoryStream();
            await new WriterToXml(stream).GetXmlAsync(productNames, "Names", "name");
            await using var command = new SqlCommand("SelectProductsByNames", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            const string productNamesParameter = "@productNames";
            command.Parameters.Add(productNamesParameter, SqlDbType.Xml);
            command.Parameters[productNamesParameter].Value = new SqlXml(stream);

            await this.connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                yield return CreateProduct(reader);
            }
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<ProductTransferObject> SelectProductsByCategoryAsync(ICollection<int> collectionOfCategoryId)
        {
            TaskArgumentVerificator.CheckItemIsNull(collectionOfCategoryId);
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 1, collectionOfCategoryId.Count, "Collection is empty.");

            await using var stream = new MemoryStream();
            await new WriterToXml(stream).GetXmlAsync(collectionOfCategoryId, "CategoryIds", "idCategory");
            await using var command = new SqlCommand("SelectProductsByCategoryIds", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            const string productNamesParameter = "@categoryIds";
            command.Parameters.Add(productNamesParameter, SqlDbType.Xml);
            command.Parameters[productNamesParameter].Value = new SqlXml(stream);

            await this.connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                yield return CreateProduct(reader);
            }
        }

        private static ProductTransferObject CreateProduct(SqlDataReader reader)
        {
            var id = (int)reader["ProductID"];
            var name = (string)reader["ProductName"];

            const string supplierIdColumnName = "SupplierID";
            int? supplierId = reader[supplierIdColumnName] != DBNull.Value ? (int)reader[supplierIdColumnName] : null;

            const string categoryIdColumnName = "CategoryID";
            int? categoryId = reader[categoryIdColumnName] != DBNull.Value ? (int)reader[categoryIdColumnName] : null;

            const string quantityPerUnitColumnName = "QuantityPerUnit";
            string quantityPerUnit = reader[quantityPerUnitColumnName] != DBNull.Value ? (string)reader[quantityPerUnitColumnName] : null;

            const string unitPriceColumnName = "UnitPrice";
            decimal? unitPrice = reader[unitPriceColumnName] != DBNull.Value ? (decimal)reader[unitPriceColumnName] : null;

            const string unitsInStockColumnName = "UnitsInStock";
            short? unitsInStock = reader[unitsInStockColumnName] != DBNull.Value ? (short)reader[unitsInStockColumnName] : null;

            const string unitsOnOrderColumnName = "UnitsOnOrder";
            short? unitsOnOrder = reader[unitsOnOrderColumnName] != DBNull.Value ? (short)reader[unitsOnOrderColumnName] : null;

            const string reorderLevelColumnName = "ReorderLevel";
            short? reorderLevel = reader[reorderLevelColumnName] != DBNull.Value ? (short)reader[reorderLevelColumnName] : null;

            const string discontinuedColumnName = "Discontinued";
            bool discontinued = (bool)reader[discontinuedColumnName];

            return new ProductTransferObject
            {
                Id = id,
                Name = name,
                SupplierId = supplierId,
                CategoryId = categoryId,
                QuantityPerUnit = quantityPerUnit,
                UnitPrice = unitPrice,
                UnitsInStock = unitsInStock,
                UnitsOnOrder = unitsOnOrder,
                ReorderLevel = reorderLevel,
                Discontinued = discontinued,
            };
        }

        private static void AddSqlParameters(ProductTransferObject product, SqlCommand command)
        {
            const string productNameParameter = "@productName";
            command.Parameters.Add(productNameParameter, SqlDbType.NVarChar, 40);
            command.Parameters[productNameParameter].Value = product.Name;

            const string supplierIdParameter = "@supplierId";
            command.Parameters.Add(supplierIdParameter, SqlDbType.Int);
            command.Parameters[supplierIdParameter].IsNullable = true;
            command.Parameters[supplierIdParameter].Value = product.SupplierId is null ? DBNull.Value : product.SupplierId;

            const string categoryIdParameter = "@categoryId";
            command.Parameters.Add(categoryIdParameter, SqlDbType.Int);
            command.Parameters[categoryIdParameter].IsNullable = true;
            command.Parameters[categoryIdParameter].Value = product.CategoryId is null ? DBNull.Value : product.CategoryId;

            const string quantityPerUnitParameter = "@quantityPerUnit";
            command.Parameters.Add(quantityPerUnitParameter, SqlDbType.NVarChar, 20);
            command.Parameters[quantityPerUnitParameter].IsNullable = true;
            command.Parameters[quantityPerUnitParameter].Value = product.QuantityPerUnit is null ? DBNull.Value : product.QuantityPerUnit;

            const string unitPriceParameter = "@unitPrice";
            command.Parameters.Add(unitPriceParameter, SqlDbType.Money);
            command.Parameters[unitPriceParameter].IsNullable = true;
            command.Parameters[unitPriceParameter].Value = product.UnitPrice is null ? DBNull.Value : product.UnitPrice;

            const string unitsInStockParameter = "@unitsInStock";
            command.Parameters.Add(unitsInStockParameter, SqlDbType.SmallInt);
            command.Parameters[unitsInStockParameter].IsNullable = true;
            command.Parameters[unitsInStockParameter].Value = product.UnitsInStock is null ? DBNull.Value : product.UnitsInStock;

            const string unitsOnOrderParameter = "@unitsOnOrder";
            command.Parameters.Add(unitsOnOrderParameter, SqlDbType.SmallInt);
            command.Parameters[unitsOnOrderParameter].IsNullable = true;
            command.Parameters[unitsOnOrderParameter].Value = product.UnitsOnOrder is null ? DBNull.Value : product.UnitsOnOrder;

            const string reorderLevelParameter = "@reorderLevel";
            command.Parameters.Add(reorderLevelParameter, SqlDbType.SmallInt);
            command.Parameters[reorderLevelParameter].IsNullable = true;
            command.Parameters[reorderLevelParameter].Value = product.ReorderLevel is null ? DBNull.Value : product.ReorderLevel;

            const string discontinuedParameter = "@discontinued";
            command.Parameters.Add(discontinuedParameter, SqlDbType.Bit);
            command.Parameters[discontinuedParameter].Value = product.Discontinued;
        }
    }
}
