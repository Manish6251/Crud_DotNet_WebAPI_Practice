using Demo_Api.Model;

namespace Demo_Api.Repository
{
         public interface IEmployeeRepository
        {
            Task<bool> CreateAsync(Employee employee);
            Task<IEnumerable<Employee>> GetAllAsync();
            Task<Employee> GetByIdAsync(int id);
            Task<bool> UpdateAsync(Employee employee);
            Task<bool> DeleteAsync(int id);
        }

}
