using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SreExercise.Web.Models;
using SreExercise.Web.Models.Data;

namespace SreExercise.Web.Controllers
{
    /// <summary>
    /// Employee Data Management API.
    /// </summary>
    /// <response code="401">Invalid API Key.</response>
    [Route("api/employees")]
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class EmployeeApiController : ControllerBase
    {
        private readonly EmployeeDal _dal;

        public EmployeeApiController(EmployeeDal dal)
        {
            _dal = dal ?? throw new ArgumentNullException(nameof(dal));
        }

        /// <summary>
        /// Get all employees data.
        /// </summary>
        /// <response code="200">All employees data.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _dal.GetAllAsync();
            var responseData = AllEmployeesResponse.FromData(employees);
            return Ok(responseData);
        }

        /// <summary>
        /// Get one employee data with specific ID.
        /// </summary>
        /// <response code="200">One employees data.</response>
        /// <response code="404">Employee not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSingle([FromRoute] string id)
        {
            var employee = await _dal.GetAsync(id);
            if(employee == null)
            {
                return NotFound();
            }

            var responseData = SingleEmployeeResponse.FromData(employee);
            return Ok(responseData);
        }

        /// <summary>
        /// Create new employee.
        /// </summary>
        /// <response code="201">The uri of new employee in the header 'Location'.</response>
        /// <response code="400">The employee data are invalid.</response>
        /// <response code="409">The employee ID duplicated.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeRequest requestData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _dal.TryCreateAsync(requestData.ToEmployee()))
            {
                return CreatedAtAction(nameof(GetSingle), new { id = requestData.Id }, null);
            }
            else
            {
                return Conflict();
            }
        }

        /// <summary>
        /// Update one employee data with specific ID.
        /// </summary>
        /// <response code="204">The update operation is success.</response>
        /// <response code="400">The employee data are invalid.</response>
        /// <response code="404">The employee not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateEmployeeRequest requestData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _dal.GetAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            requestData.UpdateEmployee(employee);
            await _dal.UpdateAsync(employee);
            return NoContent();
        }

        /// <summary>
        /// Delete one employee data with specific ID.
        /// </summary>
        /// <response code="204">The delete operation succeeded.</response>
        /// <response code="404">The employee not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var employee = await _dal.GetAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            await _dal.DeleteAsync(employee);
            return NoContent();
        }
    }
}