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
        private IEmployeeRepository employeeRepository;
        private IMapper mapper;
        public EmployeeService(EmployeeRepository employeeRepository, IMapper mapper)
        {
            this.employeeRepository = employeeRepository;
            this.mapper = mapper;
        }
        public async Task Add(EmployeeDTO employee)
        {
            var mappedEmployee = mapper.Map<Employee>(employee);
            if (employee != null)
            {
                await employeeRepository.Create(mappedEmployee);
            }
        }

        public async Task<bool> Delete(int id)
        {
            return await employeeRepository.Delete(id);
        }

        public async Task<List<EmployeeDTO>> GetAll()
        {
            var employees = await employeeRepository.ReadAll();
            var mappedEmployees = employees.Select(q => mapper.Map<EmployeeDTO>(q)).ToList();
            return mappedEmployees;
        }

        public async Task<EmployeeDTO?> GetById(int id)
        {
            var employee = await employeeRepository.ReadById(id);
            var mappedEmployee = mapper.Map<EmployeeDTO>(employee);
            return mappedEmployee;
        }

        public async Task<bool> Update(EmployeeDTO employee)
        {
            var mappedemploee = mapper.Map<Employee>(employee);
            return await employeeRepository.Update(mappedemploee);
        }
    }
}
