using Application.DTOs;
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
        public EmployeeService(EmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }
        public Task Create(EmployeeDTO employee)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<EmployeeDTO>> ReadAll()
        {
            throw new NotImplementedException();
        }

        public Task<EmployeeDTO?> ReadById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(EmployeeDTO employee)
        {
            throw new NotImplementedException();
        }
    }
}
