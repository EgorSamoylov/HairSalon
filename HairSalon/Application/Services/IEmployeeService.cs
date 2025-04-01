using Application.DTOs;
using Application.Request.EmployeeRequest;

namespace Application.Services
{
    public interface IEmployeeService
    {
        public Task<EmployeeDTO?> GetById(int id);
        public Task<IEnumerable<EmployeeDTO>> GetAll();
        public Task Add(CreateEmployeeRequest employee);
        public Task<bool> Update(UpdateEmployeeRequest employee);
        public Task<bool> Delete(int id);
    }
}
