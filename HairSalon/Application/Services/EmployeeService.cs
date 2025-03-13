using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<int> Add(EmployeeDTO employee)
        {
            var mappedEmployee = _mapper.Map<Employee>(employee);
            if (employee != null)
            {
                await _employeeRepository.Create(mappedEmployee);
                return mappedEmployee.Id;
            }

            throw new ArgumentException("Failed to map EmployeeDTO to Employee"); //Или return -1;
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

        public async Task<bool> Update(EmployeeDTO employee)
        {
            var mappedemploee = _mapper.Map<Employee>(employee);
            return await _employeeRepository.Update(mappedemploee);
        }
    }
}
