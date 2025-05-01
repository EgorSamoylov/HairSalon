using Application.DTOs;
using Application.Request;
using Application.Request.EmployeeRequest;

namespace Application.Services
{
    public interface IEmployeeService
    {
        public Task<EmployeeDTO> GetById(int id);
        public Task<IEnumerable<EmployeeDTO>> GetAll();
        public Task<int> Add(CreateEmployeeRequest employee);
        public Task<int> Add(RegistrationRequest request);
        public Task Update(UpdateEmployeeRequest employee);
        public Task Delete(int id);
    }
}
