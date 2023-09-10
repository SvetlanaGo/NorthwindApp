using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Northwind.DataAccess.Employees;

namespace Northwind.DataAccess.SqlServer.Employees
{
    /// <summary>
    /// Represents a SQL Server-tailored DAO for Northwind employees.
    /// </summary>
    public sealed class EmployeeSqlServerDataAccessObject : IEmployeeDataAccessObject
    {
        private readonly SqlConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeSqlServerDataAccessObject"/> class.
        /// </summary>
        /// <param name="connection">A <see cref="SqlConnection"/>.</param>
        /// <exception cref="ArgumentNullException">Throw when connection is null.</exception>
        public EmployeeSqlServerDataAccessObject(SqlConnection connection) =>
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));

        /// <inheritdoc/>
        public async Task<int> InsertEmployeeAsync(EmployeeTransferObject employee)
        {
            TaskArgumentVerificator.CheckItemIsNull(employee);

            await using var command = new SqlCommand("InsertEmployee", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            AddSqlParameters(employee, command);
            await this.connection.OpenAsync();
            var id = await command.ExecuteScalarAsync();

            return (int)id;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteEmployeeAsync(int employeeId)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, employeeId, "Must be greater than zero.");

            await using var command = new SqlCommand("DeleteEmployee", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            const string employeeIdParameter = "@employeeID";
            command.Parameters.Add(employeeIdParameter, SqlDbType.Int);
            command.Parameters[employeeIdParameter].Value = employeeId;
            await this.connection.OpenAsync();
            var result = await command.ExecuteNonQueryAsync();

            return result > 0;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateEmployeeAsync(EmployeeTransferObject employee)
        {
            TaskArgumentVerificator.CheckItemIsNull(employee);

            await using var command = new SqlCommand("UpdateEmployee", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            AddSqlParameters(employee, command);

            const string employeeId = "@employeeId";
            command.Parameters.Add(employeeId, SqlDbType.Int);
            command.Parameters[employeeId].Value = employee.Id;
            await this.connection.OpenAsync();
            var result = await command.ExecuteNonQueryAsync();

            return result > 0;
        }

        /// <inheritdoc/>
        public async Task<EmployeeTransferObject> FindEmployeeAsync(int employeeId)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, employeeId, "Must be greater than zero.");

            await using var command = new SqlCommand("FindEmployee", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            const string employeeIdParameter = "@employeeId";
            command.Parameters.Add(employeeIdParameter, SqlDbType.Int);
            command.Parameters[employeeIdParameter].Value = employeeId;
            await this.connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
            {
                throw new EmployeeNotFoundException(employeeId);
            }

            var employee = CreateEmployee(reader);
            await this.connection.CloseAsync();

            return employee;
        }

        /// <inheritdoc />
        public async IAsyncEnumerable<EmployeeTransferObject> SelectEmployeesAsync(int offset, int limit)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 0, offset, "Must be greater than zero or equals zero.");
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 1, limit, "Must be greater than zero.");

            await using var command = new SqlCommand("SelectEmployees", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            const string offsetEmployees = "@offsetEmployees";
            command.Parameters.Add(offsetEmployees, SqlDbType.Int);
            command.Parameters[offsetEmployees].Value = offset;

            const string limitEmployees = "@limitEmployees";
            command.Parameters.Add(limitEmployees, SqlDbType.Int);
            command.Parameters[limitEmployees].Value = limit;

            await this.connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                yield return CreateEmployee(reader);
            }
        }

        private static void AddSqlParameters(EmployeeTransferObject employee, SqlCommand command)
        {
            const string lastNameParameter = "@lastName";
            command.Parameters.Add(lastNameParameter, SqlDbType.NVarChar, 20);
            command.Parameters[lastNameParameter].Value = employee.LastName;

            const string firstNameParameter = "@firstName";
            command.Parameters.Add(firstNameParameter, SqlDbType.NVarChar, 10);
            command.Parameters[firstNameParameter].Value = employee.FirstName;

            const string titleParameter = "@title";
            command.Parameters.Add(titleParameter, SqlDbType.NVarChar, 30);
            command.Parameters[titleParameter].IsNullable = true;
            command.Parameters[titleParameter].Value = employee.Title is null ? DBNull.Value : employee.Title;

            const string titleOfCourtesyParameter = "@titleOfCourtesy";
            command.Parameters.Add(titleOfCourtesyParameter, SqlDbType.NVarChar, 25);
            command.Parameters[titleOfCourtesyParameter].IsNullable = true;
            command.Parameters[titleOfCourtesyParameter].Value = employee.TitleOfCourtesy is null ? DBNull.Value : employee.TitleOfCourtesy;

            const string birthDateParameter = "@birthDate";
            command.Parameters.Add(birthDateParameter, SqlDbType.DateTime);
            command.Parameters[birthDateParameter].IsNullable = true;
            command.Parameters[birthDateParameter].Value = employee.BirthDate is null ? DBNull.Value : employee.BirthDate;

            const string hireDateParameter = "@hireDate";
            command.Parameters.Add(hireDateParameter, SqlDbType.DateTime);
            command.Parameters[hireDateParameter].IsNullable = true;
            command.Parameters[hireDateParameter].Value = employee.HireDate is null ? DBNull.Value : employee.HireDate;

            const string addressParameter = "@address";
            command.Parameters.Add(addressParameter, SqlDbType.NVarChar, 60);
            command.Parameters[addressParameter].IsNullable = true;
            command.Parameters[addressParameter].Value = employee.Address is null ? DBNull.Value : employee.Address;

            const string cityParameter = "@city";
            command.Parameters.Add(cityParameter, SqlDbType.NVarChar, 15);
            command.Parameters[cityParameter].IsNullable = true;
            command.Parameters[cityParameter].Value = employee.City is null ? DBNull.Value : employee.City;

            const string regionParameter = "@region";
            command.Parameters.Add(regionParameter, SqlDbType.NVarChar, 15);
            command.Parameters[regionParameter].IsNullable = true;
            command.Parameters[regionParameter].Value = employee.Region is null ? DBNull.Value : employee.Region;

            const string postalCodeParameter = "@postalCode";
            command.Parameters.Add(postalCodeParameter, SqlDbType.NVarChar, 10);
            command.Parameters[regionParameter].IsNullable = true;
            command.Parameters[postalCodeParameter].Value = employee.PostalCode is null ? DBNull.Value : employee.PostalCode;

            const string countryParameter = "@country";
            command.Parameters.Add(countryParameter, SqlDbType.NVarChar, 15);
            command.Parameters[countryParameter].IsNullable = true;
            command.Parameters[countryParameter].Value = employee.Country is null ? DBNull.Value : employee.Country;

            const string homePhoneParameter = "@homePhone";
            command.Parameters.Add(homePhoneParameter, SqlDbType.NVarChar, 24);
            command.Parameters[homePhoneParameter].IsNullable = true;
            command.Parameters[homePhoneParameter].Value = employee.HomePhone is null ? DBNull.Value : employee.HomePhone;

            const string extensionParameter = "@extension";
            command.Parameters.Add(extensionParameter, SqlDbType.NVarChar, 4);
            command.Parameters[extensionParameter].IsNullable = true;
            command.Parameters[extensionParameter].Value = employee.Extension is null ? DBNull.Value : employee.HomePhone;

            const string photoParameter = "@photo";
            command.Parameters.Add(photoParameter, SqlDbType.Image);
            command.Parameters[photoParameter].IsNullable = true;
            command.Parameters[photoParameter].Value = employee.Photo is null ? DBNull.Value : employee.Photo;

            const string notesParameter = "@notes";
            command.Parameters.Add(notesParameter, SqlDbType.Text);
            command.Parameters[notesParameter].IsNullable = true;
            command.Parameters[notesParameter].Value = employee.Notes is null ? DBNull.Value : employee.Notes;

            const string reportsToParameter = "@reportsTo";
            command.Parameters.Add(reportsToParameter, SqlDbType.Int);
            command.Parameters[reportsToParameter].IsNullable = true;
            command.Parameters[reportsToParameter].Value = employee.ReportsTo is null ? DBNull.Value : employee.ReportsTo;

            const string photoPathParameter = "@photoPath";
            command.Parameters.Add(photoPathParameter, SqlDbType.NVarChar, 255);
            command.Parameters[photoPathParameter].IsNullable = true;
            command.Parameters[photoPathParameter].Value = employee.PhotoPath is null ? DBNull.Value : employee.PhotoPath;
        }

        private static EmployeeTransferObject CreateEmployee(SqlDataReader reader)
        {
            var id = (int)reader["EmployeeID"];
            var lastName = (string)reader["LastName"];
            var firstName = (string)reader["FirstName"];

            const string TitleColumnName = "Title";
            string title = reader[TitleColumnName] != DBNull.Value ? (string)reader[TitleColumnName] : null;

            const string TitleOfCourtesyColumnName = "TitleOfCourtesy";
            string titleOfCourtesy = reader[TitleOfCourtesyColumnName] != DBNull.Value ? (string)reader[TitleOfCourtesyColumnName] : null;

            const string BirthDateColumnName = "BirthDate";
            DateTime? birthDate = reader[BirthDateColumnName] != DBNull.Value ? (DateTime)reader[BirthDateColumnName] : null;

            const string HireDateColumnName = "HireDate";
            DateTime? hireDate = reader[HireDateColumnName] != DBNull.Value ? (DateTime)reader[HireDateColumnName] : null;

            const string AddressColumnName = "Address";
            string address = reader[AddressColumnName] != DBNull.Value ? (string)reader[AddressColumnName] : null;

            const string CityColumnName = "City";
            string city = reader[CityColumnName] != DBNull.Value ? (string)reader[CityColumnName] : null;

            const string RegionColumnName = "Region";
            string region = reader[RegionColumnName] != DBNull.Value ? (string)reader[RegionColumnName] : null;

            const string PostalCodeColumnName = "PostalCode";
            string postalCode = reader[PostalCodeColumnName] != DBNull.Value ? (string)reader[PostalCodeColumnName] : null;

            const string CountryColumnName = "Country";
            string country = reader[CountryColumnName] != DBNull.Value ? (string)reader[CountryColumnName] : null;

            const string HomePhoneColumnName = "HomePhone";
            string homePhone = reader[HomePhoneColumnName] != DBNull.Value ? (string)reader[HomePhoneColumnName] : null;

            const string ExtensionColumnName = "Extension";
            string extension = reader[ExtensionColumnName] != DBNull.Value ? (string)reader[ExtensionColumnName] : null;

            const string PhotoColumnName = "Photo";
            byte[] photo = reader[PhotoColumnName] != DBNull.Value ? (byte[])reader[PhotoColumnName] : null;

            const string NotesColumnName = "Notes";
            string notes = reader[NotesColumnName] != DBNull.Value ? (string)reader[NotesColumnName] : null;

            const string ReportsToColumnName = "ReportsTo";
            int? reportsTo = reader[ReportsToColumnName] != DBNull.Value ? (int)reader[ReportsToColumnName] : null;

            const string PhotoPathColumnName = "PhotoPath";
            string photoPath = reader[PhotoPathColumnName] != DBNull.Value ? (string)reader[PhotoPathColumnName] : null;

            return new EmployeeTransferObject
            {
                Id = id,
                LastName = lastName,
                FirstName = firstName,
                Title = title,
                TitleOfCourtesy = titleOfCourtesy,
                BirthDate = birthDate,
                HireDate = hireDate,
                Address = address,
                City = city,
                Region = region,
                PostalCode = postalCode,
                Country = country,
                HomePhone = homePhone,
                Extension = extension,
                Photo = photo,
                Notes = notes,
                ReportsTo = reportsTo,
                PhotoPath = photoPath,
            };
        }
    }
}
