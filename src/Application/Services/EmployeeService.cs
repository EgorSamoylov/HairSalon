using Application.DTOs;
using Application.Exceptions;
using Application.Request.EmployeeRequest;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.EmployeeRepository;

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

        public async Task<int> Add(CreateEmployeeRequest request)
        {
            var employee = new Employee()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Position = request.Position
            };

            return await _employeeRepository.Create(employee);
        }

        public async Task Delete(int id)
        {
            var result = await _employeeRepository.Delete(id);
            if (!result)
            {
                throw new EntityDeleteException("Employee not found");
            }
        }

        public async Task<IEnumerable<EmployeeDTO>> GetAll()
        {
            var employees = await _employeeRepository.ReadAll();
            var mappedEmployees = employees.Select(q => _mapper.Map<EmployeeDTO>(q));
            return mappedEmployees;
        }

        public async Task<EmployeeDTO> GetById(int id)
        {
            var employee = await _employeeRepository.ReadById(id);
            if (employee is null)
            {
                throw new NotFoundApplicationException("Employee not found");
            }
            var mappedEmployee = _mapper.Map<EmployeeDTO>(employee);
            return mappedEmployee;
        }

        public async Task Update(UpdateEmployeeRequest request)
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

            var result = await _employeeRepository.Update(employee);
            if (!result)
            {
                throw new EntityUpdateException("Employee wasn't updated");
            }
        }
    }
}
