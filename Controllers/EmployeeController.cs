using Demo_Api.Model;
using Demo_Api.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Demo_Api.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpPost("postEmployee")]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (await _employeeRepository.CreateAsync(employee))
            {
                return Json(new { success = true, message = "Employee created successfully!" });
            }

            return Json(new { success = false, message = "Validation failed. Please check your input." });
        }

        [HttpGet("getEmployee")]
        public async Task<IActionResult> Get()
        {
            var employees = await _employeeRepository.GetAllAsync();

            if (employees == null || !employees.Any())
            {
                return NotFound();
            }

            return Ok(employees);
        }

        [HttpGet("getEmployeeById")]
        public async Task<IActionResult> GetById([FromQuery] int Id)
        {
            if (Id <= 0)
            {
                return BadRequest("Invalid Employee Id.");
            }

            var employee = await _employeeRepository.GetByIdAsync(Id);

            if (employee == null)
            {
                return Json(new { success = true, message = "Employee Id not found!" });
            }

            return Ok(employee);
        }

        [HttpPut("updateEmployee")]
        public async Task<IActionResult> Update(Employee employee)
        {
            if (employee.Id == null)
            {
                return Json(new { success = false, message = "Employee ID is required for update." });
            }

            if (await _employeeRepository.UpdateAsync(employee))
            {
                return Json(new { success = true, message = "Employee updated successfully!" });
            }
            else
            {
                return Json(new { success = false, message = "Employee not found or no changes were made." });
            }
        }

        [HttpDelete("deleteEmployee")]
        public async Task<IActionResult> Delete([FromQuery] int Id)
        {
            if (Id <= 0)
            {
                return BadRequest("Invalid ID.");
            }

            if (await _employeeRepository.DeleteAsync(Id))
            {
                return Ok("Employee deleted successfully.");
            }

            return Json(new { success = false, message = "Employee not found or no changes were made." });
        }
    }
}
