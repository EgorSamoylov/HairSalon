using Application.DTOs;
using Application.Request.EmployeeRequest;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories;

namespace Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task Add(CreateEmployeeRequest request)
        {
            var employee = new Employee()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Position = request.Position
            };

            await _employeeRepository.Create(employee);
        }

        public async Task<bool> Delete(int id)
        {
            return await _employeeRepository.Delete(id);
        }

        public async Task<IEnumerable<EmployeeDTO>> GetAll()
        {
            var employees = await _employeeRepository.ReadAll();
            var mappedEmployees = employees.Select(q => _mapper.Map<EmployeeDTO>(q)).ToList();
            return mappedEmployees;
        }

        public async Task<EmployeeDTO?> GetById(int id)
        {
            var employee = await _employeeRepository.ReadById(id);
            var mappedEmployee = _mapper.Map<EmployeeDTO>(employee);
            return mappedEmployee;
        }

        public async Task<bool> Update(UpdateEmployeeRequest request)
        {
            var employee = new Employee()
            {
                Id = request.EmployeeId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Position = request.Position
            };

            return await _employeeRepository.Update(employee);
        }
    }
}
