using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SreExercise.Web.Models;
using SreExercise.Web.Models.Data;

namespace SreExercise.Web.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeeApiController : ControllerBase
    {
        private readonly EmployeeDal _dal;

        public EmployeeApiController(EmployeeDal dal)
        {
            _dal = dal ?? throw new ArgumentNullException(nameof(dal));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _dal.GetAllAsync();
            var responseData = AllEmployeesResponse.FromData(employees);
            return Ok(responseData);
        }

        [HttpGet("{id}")]
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

        [HttpPost]
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

        [HttpPut("{id}")]
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

        [HttpDelete("{id}")]
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