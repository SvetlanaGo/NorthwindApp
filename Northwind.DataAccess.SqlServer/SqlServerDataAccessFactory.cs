using System;
using System.Data.SqlClient;
using Northwind.DataAccess.Employees;
using Northwind.DataAccess.Products;
using Northwind.DataAccess.SqlServer.Employees;
using Northwind.DataAccess.SqlServer.Products;

namespace Northwind.DataAccess.SqlServer
{
    /// <summary>
    /// Represents an abstract factory for creating Northwind DAO for SQL Server.
    /// </summary>
    public sealed class SqlServerDataAccessFactory : NorthwindDataAccessFactory
    {
        private readonly SqlConnection sqlConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerDataAccessFactory"/> class.
        /// </summary>
        /// <param name="sqlConnection">A database connection to SQL Server.</param>
        /// <exception cref="ArgumentNullException">Throw when connection is null.</exception>
        public SqlServerDataAccessFactory(SqlConnection sqlConnection) =>
            this.sqlConnection = sqlConnection ?? throw new ArgumentNullException(nameof(sqlConnection));

        /// <inheritdoc/>
        public override IProductCategoryDataAccessObject GetProductCategoryDataAccessObject() =>
            new ProductCategorySqlServerDataAccessObject(this.sqlConnection);

        /// <inheritdoc/>
        public override IProductDataAccessObject GetProductDataAccessObject() =>
            new ProductSqlServerDataAccessObject(this.sqlConnection);

        /// <inheritdoc />
        public override IEmployeeDataAccessObject GetEmployeeDataAccessObject() =>
            new EmployeeSqlServerDataAccessObject(this.sqlConnection);
    }
}
