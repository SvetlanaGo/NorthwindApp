using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Employees;

namespace NorthwindApiApp.Controllers
{
    /// <summary>
    /// Class EmployeesController.
    /// </summary>
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeManagementService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeesController"/> class.
        /// </summary>
        /// <param name="service">Employee management service.</param>
        /// <exception cref="ArgumentNullException">Throw when service is null.</exception>
        public EmployeesController(IEmployeeManagementService service) =>
            this.service = service ?? throw new ArgumentNullException(nameof(service));

        /// <summary>
        /// Creates a new employee.
        /// </summary>
        /// <param name="employee">A <see cref="EmployeeModel"/> to create.</param>
        /// <returns>An identifier of a created employee.</returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmployeeModel>> CreateEmployeeAsync(EmployeeModel employee)
        {
            if (employee is null)
            {
                return this.BadRequest();
            }

            var employeeId = await this.service.CreateEmployeeAsync(employee);

            return this.CreatedAtAction(nameof(this.ReadEmployeeAsync), new { id = employeeId }, employee);
        }

        /// <summary>
        /// Destroys an existed employee.
        /// </summary>
        /// <param name="id">An employee identifier.</param>
        /// <returns>True if an employee is destroyed; otherwise false.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEmployeeAsync([FromRoute] int id)
        {
            if (id <= 0)
            {
                return this.BadRequest();
            }

            var result = await this.service.DestroyEmployeeAsync(id);

            return result ? this.NoContent() : this.NotFound();
        }

        /// <summary>
        /// Updates an employee.
        /// </summary>
        /// <param name="id">A employee identifier.</param>
        /// <param name="employee">A <see cref="EmployeeModel"/>.</param>
        /// <returns>True if an employee is updated; otherwise false.</returns>
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateEmployeeAsync([FromRoute] int id, EmployeeModel employee)
        {
            if (employee is null || id <= 0)
            {
                return this.BadRequest();
            }

            var result = await this.service.UpdateEmployeeAsync(id, employee);

            return result ? this.NoContent() : this.NotFound();
        }

        /// <summary>
        /// Show an employee with specified identifier.
        /// </summary>
        /// <param name="id">An employee identifier.</param>
        /// <returns>Returns true if an employee is returned; otherwise false.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeModel>> ReadEmployeeAsync([FromRoute] int id)
        {
            if (id < 0)
            {
                return this.BadRequest();
            }

            var employee = await this.service.GetEmployeeAsync(id);

            return employee != null ? this.Ok(employee) : this.NotFound();
        }

        /// <summary>
        /// Shows a list of employee using specified offset and limit for pagination.
        /// </summary>
        /// <param name="offset">An offset of the first element to return.</param>
        /// <param name="limit">A limit of elements to return.</param>
        /// <returns>A <see cref="IEnumerable{T}"/> of <see cref="EmployeeModel"/>.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async IAsyncEnumerable<EmployeeModel> ReadEmployeesAsync([FromQuery] int offset = 0, [FromQuery] int limit = 10)
        {
            await foreach (var employee in this.service.GetEmployeesAsync(offset, limit))
            {
                yield return employee;
            }
        }
    }
}
