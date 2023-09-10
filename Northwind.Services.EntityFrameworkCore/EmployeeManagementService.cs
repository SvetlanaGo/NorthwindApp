using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Northwind.Services.Employees;
using Northwind.Services.EntityFrameworkCore.Context;
using Northwind.Services.EntityFrameworkCore.Entities;

namespace Northwind.Services.EntityFrameworkCore
{
    /// <summary>
    /// Represents an employee management service.
    /// </summary>
    public sealed class EmployeeManagementService : IEmployeeManagementService
    {
        private readonly NorthwindContext context;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeManagementService"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="mapper">A <see cref="IMapper"/>.</param>
        /// <exception cref="ArgumentNullException">Throw when context is null.</exception>
        public EmployeeManagementService(NorthwindContext context, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc/>
        public async Task<int> CreateEmployeeAsync(EmployeeModel employee)
        {
            TaskArgumentVerificator.CheckItemIsNull(employee);

            var transfer = this.mapper.Map<Employee>(employee);
            await this.context.Employees.AddAsync(transfer).ConfigureAwait(false);
            await this.context.SaveChangesAsync();

            return transfer.EmployeeId;
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyEmployeeAsync(int employeeId)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, employeeId, "Must be greater than zero.");

            var employee = await this.context.Employees.FindAsync(employeeId);
            if (employee is null)
            {
                return false;
            }

            this.context.Employees.Remove(employee);
            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateEmployeeAsync(int employeeId, EmployeeModel employee)
        {
            TaskArgumentVerificator.CheckItemIsNull(employee);
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, employeeId, "Must be greater than zero.");

            var employeeUp = await this.context.Employees.FindAsync(employeeId);
            if (employeeUp is null)
            {
                return false;
            }

            employeeUp.LastName = employee.LastName;
            employeeUp.FirstName = employee.FirstName;
            employeeUp.Title = employee.Title;
            employeeUp.TitleOfCourtesy = employee.TitleOfCourtesy;
            employeeUp.BirthDate = employee.BirthDate;
            employeeUp.HireDate = employee.HireDate;
            employeeUp.Address = employee.Address;
            employeeUp.City = employee.City;
            employeeUp.Region = employee.Region;
            employeeUp.PostalCode = employee.PostalCode;
            employeeUp.Country = employee.Country;
            employeeUp.HomePhone = employee.HomePhone;
            employeeUp.Extension = employee.Extension;
            employeeUp.Photo = employee.Photo;
            employeeUp.Notes = employee.Notes;
            employeeUp.ReportsTo = employee.ReportsTo;
            employeeUp.PhotoPath = employee.PhotoPath;

            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        /// <inheritdoc/>
        public async Task<EmployeeModel> GetEmployeeAsync(int employeeId)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x <= 0, employeeId, "Must be greater than zero.");

            return this.mapper.Map<EmployeeModel>(await this.context.Employees.FindAsync(employeeId));
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<EmployeeModel> GetEmployeesAsync(int offset, int limit)
        {
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 0, offset, "Must be greater than zero or equals zero.");
            TaskArgumentVerificator.CheckIntegerMoreLess(x => x < 1, limit, "Must be greater than zero.");

            await foreach (var employee in this.context.Employees.OrderBy(m => m.EmployeeId).Skip(offset).Take(limit).AsAsyncEnumerable())
            {
                yield return this.mapper.Map<EmployeeModel>(employee);
            }
        }
    }
}
