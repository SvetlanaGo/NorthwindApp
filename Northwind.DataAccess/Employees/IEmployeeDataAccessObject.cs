using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.DataAccess.Employees
{
    /// <summary>
    /// Represents a DAO for Northwind employees.
    /// </summary>
    public interface IEmployeeDataAccessObject
    {
        /// <summary>
        /// Inserts a new Northwind employee to a data storage.
        /// </summary>
        /// <param name="employee">A <see cref="EmployeeTransferObject"/>.</param>
        /// <returns>A data storage identifier of a new employee.</returns>
        /// <exception cref="ArgumentNullException">Throw when employee is null.</exception>
        Task<int> InsertEmployeeAsync(EmployeeTransferObject employee);

        /// <summary>
        /// Deletes a Northwind employee from a data storage.
        /// </summary>
        /// <param name="employeeId">An employee identifier.</param>
        /// <returns>True if a employee is deleted; otherwise false.</returns>
        /// <exception cref="ArgumentException">Throw when employee identifier is less than or equal to zero.</exception>
        Task<bool> DeleteEmployeeAsync(int employeeId);

        /// <summary>
        /// Updates a Northwind employee in a data storage.
        /// </summary>
        /// <param name="employee">A <see cref="EmployeeTransferObject"/>.</param>
        /// <returns>True if a employee is updated; otherwise false.</returns>
        /// <exception cref="ArgumentNullException">Throw when employee is null.</exception>
        Task<bool> UpdateEmployeeAsync(EmployeeTransferObject employee);

        /// <summary>
        /// Finds a Northwind employee using a specified identifier.
        /// </summary>
        /// <param name="employeeId">A data storage identifier of an existed employee.</param>
        /// <returns>A <see cref="EmployeeTransferObject"/> with specified identifier.</returns>
        /// <exception cref="ArgumentException">Throw when employeeId is less than or equal to zero.</exception>
        /// <exception cref="EmployeeNotFoundException">Throw when employee is not found.</exception>
        Task<EmployeeTransferObject> FindEmployeeAsync(int employeeId);

        /// <summary>
        /// Selects employees using specified offset and limit.
        /// </summary>
        /// <param name="offset">An offset of the first object.</param>
        /// <param name="limit">A limit of returned objects.</param>
        /// <returns>A <see cref="IAsyncEnumerable{T}"/> of <see cref="EmployeeTransferObject"/>.</returns>
        /// <exception cref="ArgumentException">Throw when offset is less than or equal to zero.</exception>
        /// <exception cref="ArgumentException">Throw when limit is less than or equal to one.</exception>
        IAsyncEnumerable<EmployeeTransferObject> SelectEmployeesAsync(int offset, int limit);
    }
}
