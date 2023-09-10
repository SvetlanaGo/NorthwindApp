using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Northwind.DataAccess;
using Northwind.DataAccess.Employees;
using Northwind.Services.Employees;

namespace Northwind.Services.DataAccess
{
    /// <summary>
    /// Represents a management service for employees.
    /// </summary>
    public sealed class EmployeeManagementDataAccessService : IEmployeeManagementService
    {
        private readonly NorthwindDataAccessFactory accessFactory;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeManagementDataAccessService"/> class.
        /// </summary>
        /// <param name="accessFactory">A <see cref="NorthwindDataAccessFactory"/>.</param>
        /// <param name="mapper">A <see cref="IMapper"/>.</param>
        /// <exception cref="ArgumentNullException">Throw when accessFactory or mapper is null.</exception>
        public EmployeeManagementDataAccessService(NorthwindDataAccessFactory accessFactory, IMapper mapper)
        {
            this.accessFactory = accessFactory ?? throw new ArgumentNullException(nameof(accessFactory));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc/>
        public async Task<int> CreateEmployeeAsync(EmployeeModel employee)
        {
            TaskArgumentVerificator.CheckItemIsNull(employee);

            return await this.accessFactory
                    .GetEmployeeDataAccessObject()
                    .InsertEmployeeAsync(this.mapper.Map<EmployeeTransferObject>(employee));
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyEmployeeAsync(int employeeId)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, employeeId, "Must be greater than zero.");

            return await this.accessFactory
                    .GetEmployeeDataAccessObject()
                    .DeleteEmployeeAsync(employeeId);
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateEmployeeAsync(int employeeId, EmployeeModel employee)
        {
            TaskArgumentVerificator.CheckItemIsNull(employee);
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, employeeId, "Must be greater than zero.");
            employee.Id = employeeId;

            return await this.accessFactory
                    .GetEmployeeDataAccessObject()
                    .UpdateEmployeeAsync(this.mapper.Map<EmployeeTransferObject>(employee));
        }

        /// <inheritdoc/>
        public async Task<EmployeeModel> GetEmployeeAsync(int employeeId)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, employeeId, "Must be greater than zero.");

            try
            {
                return this.mapper.Map<EmployeeModel>(
                    await this.accessFactory
                    .GetEmployeeDataAccessObject()
                    .FindEmployeeAsync(employeeId));
            }
            catch (EmployeeNotFoundException)
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<EmployeeModel> GetEmployeesAsync(int offset, int limit)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 0, offset, "Must be greater than zero or equals zero.");
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 1, limit, "Must be greater than zero.");

            await foreach (var transfer in this.accessFactory
                .GetEmployeeDataAccessObject()
                .SelectEmployeesAsync(offset, limit))
            {
                yield return this.mapper.Map<EmployeeModel>(transfer);
            }
        }
    }
}
