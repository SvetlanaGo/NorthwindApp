using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Services.Employees
{
    /// <summary>
    /// Represents a management service for employees.
    /// </summary>
    public interface IEmployeeManagementService
    {
        /// <summary>
        /// Creates a new employee.
        /// </summary>
        /// <param name="employee">A <see cref="EmployeeModel"/> to create.</param>
        /// <returns>An identifier of a created employee.</returns>
        /// <exception cref="ArgumentNullException">Throw when employee is null.</exception>
        Task<int> CreateEmployeeAsync(EmployeeModel employee);

        /// <summary>
        /// Destroys an existed employee.
        /// </summary>
        /// <param name="employeeId">A employee identifier.</param>
        /// <returns>True if a employee is destroyed; otherwise false.</returns>
        /// <exception cref="ArgumentException">Throw when employee identifier is less than or equal to zero.</exception>
        Task<bool> DestroyEmployeeAsync(int employeeId);

        /// <summary>
        /// Updates a employee.
        /// </summary>
        /// <param name="employeeId">A employee identifier.</param>
        /// <param name="employee">A <see cref="EmployeeModel"/>.</param>
        /// <returns>True if a employee is updated; otherwise false.</returns>
        /// <exception cref="ArgumentNullException">Throw when employee is null.</exception>
        /// <exception cref="ArgumentException">Throw when employee identifier is less than or equal to zero.</exception>
        Task<bool> UpdateEmployeeAsync(int employeeId, EmployeeModel employee);

        /// <summary>
        /// Try to show a employee with specified identifier.
        /// </summary>
        /// <param name="employeeId">A employee identifier.</param>
        /// <returns>Returns an employee.</returns>
        /// <exception cref="ArgumentException">Throw when employeeId is less than or equal to zero.</exception>
        Task<EmployeeModel> GetEmployeeAsync(int employeeId);

        /// <summary>
        /// Shows a list of employees using specified offset and limit for pagination.
        /// </summary>
        /// <param name="offset">An offset of the first element to return.</param>
        /// <param name="limit">A limit of elements to return.</param>
        /// <returns>A <see cref="IAsyncEnumerable{T}"/> of <see cref="EmployeeModel"/>.</returns>
        /// <exception cref="ArgumentException">Throw when offset is less than or equal to zero.</exception>
        /// <exception cref="ArgumentException">Throw when limit is less than or equal to one.</exception>
        IAsyncEnumerable<EmployeeModel> GetEmployeesAsync(int offset, int limit);
    }
}
