using Demo_Api.Data;
using Demo_Api.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Demo_Api.Repository
{
    public class EmployeeRepo
    {
        public class EmployeeRepository : IEmployeeRepository
        {
            private readonly ApplicationDbContext _context;

            public EmployeeRepository(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<bool> CreateAsync(Employee employee)
            {
                if (employee.Id != null)
                {
                    var parameters = new[]
                    {
                    new SqlParameter("@p_empId", SqlDbType.Int) { Value = employee.Id },
                    new SqlParameter("@p_name", SqlDbType.VarChar) { Value = employee.Name },
                    new SqlParameter("@p_position", SqlDbType.VarChar) { Value = employee.Position },
                    new SqlParameter("@p_salary", SqlDbType.VarChar) { Value = employee.Salary }
                };

                    await _context.Database.ExecuteSqlRawAsync("Insert_Emp @p_empId, @p_name, @p_position, @p_salary", parameters);
                    return true;
                }

                return false;
            }

            public async Task<IEnumerable<Employee>> GetAllAsync()
            {
                return await _context.Employees.FromSqlRaw("GetAll_Employee").ToListAsync();
            }

            public async Task<Employee> GetByIdAsync(int id)
            {
                var idParam = new SqlParameter("@p_empId", id);

                var employees = await _context.Employees
                    .FromSqlRaw("GetById_Employee @p_empId", idParam)
                    .ToListAsync();

                return employees.FirstOrDefault();
            }

            public async Task<bool> UpdateAsync(Employee employee)
            {
                if (employee.Id != null)
                {
                    var parameters = new[]
                    {
                    new SqlParameter("@p_empId", SqlDbType.Int) { Value = employee.Id },
                    new SqlParameter("@p_empName", SqlDbType.VarChar) { Value = employee.Name },
                    new SqlParameter("@p_empPosition", SqlDbType.VarChar) { Value = employee.Position },
                    new SqlParameter("@p_empSalary", SqlDbType.VarChar) { Value = employee.Salary }
                };

                    var updatedRow = await _context.Database.ExecuteSqlRawAsync(
                        "Update_Employee @p_empId, @p_empName, @p_empPosition, @p_empSalary", parameters);

                    return updatedRow > 0;
                }

                return false;
            }

            public async Task<bool> DeleteAsync(int id)
            {
                var idParam = new SqlParameter("@p_empId", id);

                var affectedRows = await _context.Database.ExecuteSqlRawAsync("Delete_employee @p_empId", idParam);

                return affectedRows > 0;
            }
        }
    }
}
