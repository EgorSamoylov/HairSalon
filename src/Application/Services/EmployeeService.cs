using Application.DTOs;
using Application.Exceptions;
using Application.Request;
using Application.Request.EmployeeRequest;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Repositories.ClientRepository;
using Infrastructure.Repositories.EmployeeRepository;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private IMapper _mapper;
        private readonly ILogger<EmployeeService> _logger;
        private IPasswordHasher hasher;
        private IAttachmentService attachmentService;

        public EmployeeService(
            IEmployeeRepository employeeRepository, 
            IMapper mapper,
            ILogger<EmployeeService> logger)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<int> Add(RegistrationRequest request)
        {
            var user = _mapper.Map<Employee>(request);
            user.PasswordHash = hasher.HashPassword(request.Password);
            user.Role = UserRoles.User;

            var userId = await _employeeRepository.Create(user);

            return userId;
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

            int employeeId = await _employeeRepository.Create(employee);
            _logger.LogInformation(
                @"Employee created with id {Id} with FirstName {FirstName}, 
                LastName {LastName},
                with PhoneNumber {PhoneNumber},
                with Email {Email},
                Position {Position}",
                employeeId,
                employee.FirstName,
                employee.LastName,
                employee.PhoneNumber,
                employee.Email,
                employee.Position);
            return employeeId;
        }

        public async Task Delete(int id)
        {
            _logger.LogInformation("Attempting to delete employee with id {Id}", id);

            var result = await _employeeRepository.Delete(id);
            if (!result)
            {
                _logger.LogWarning("Employee with id {Id} not found for deletion", id);
                throw new EntityDeleteException("Employee not found");
            }

            _logger.LogInformation("Employee with id {Id} successfully deleted", id);
        }

        public async Task<IEnumerable<EmployeeDTO>> GetAll()
        {
            _logger.LogInformation("Getting all employees");

            var employees = await _employeeRepository.ReadAll();
            var mappedEmployees = employees.Select(q => _mapper.Map<EmployeeDTO>(q));

            _logger.LogInformation("Retrieved {Count} employees", mappedEmployees.Count());
            return mappedEmployees;
        }

        public async Task<EmployeeDTO> GetById(int id)
        {
            _logger.LogInformation("Getting employee by id {Id}", id);

            var employee = await _employeeRepository.ReadById(id);
            if (employee is null)
            {
                _logger.LogWarning("Employee with id {Id} not found", id);
                throw new NotFoundApplicationException("Employee not found");
            }

            var mappedEmployee = _mapper.Map<EmployeeDTO>(employee);

            _logger.LogInformation(
                @"Retrieved employee with id {Id}: 
                FirstName {FirstName},
                LastName {LastName},
                PhoneNumber {PhoneNumber},
                Email {Email},
                Position {Position}",
                id,
                employee.FirstName,
                employee.LastName,
                employee.PhoneNumber,
                employee.Email,
                employee.Position);

            return mappedEmployee;
        }

        public async Task Update(UpdateEmployeeRequest request)
        {
            _logger.LogInformation(
                @"Attempting to update employee with id {Id}: 
                FirstName {FirstName},
                LastName {LastName},
                PhoneNumber {PhoneNumber},
                Email {Email},
                Position {Position}",
                request.EmployeeId,
                request.FirstName,
                request.LastName,
                request.PhoneNumber,
                request.Email,
                request.Position);

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
                _logger.LogWarning("Employee with id {Id} not found for update", request.EmployeeId);
                throw new EntityUpdateException("Employee wasn't updated");
            }

            _logger.LogInformation("Employee with id {Id} successfully updated", request.EmployeeId);
        }
    }
}
