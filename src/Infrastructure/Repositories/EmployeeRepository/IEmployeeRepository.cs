using Domain.Entities;

namespace Infrastructure.Repositories.EmployeeRepository
{
    public interface IEmployeeRepository
    {
        public Task<Employee?> ReadById(int id);
        public Task<IEnumerable<Employee>> ReadAll();
        public Task<int> Create(Employee employee);
        public Task<bool> Update(Employee employee);
        Task<Employee?> ReadByEmail(string email);
        public Task<bool> Delete(int id);
    }
}
