using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IEmployeeService
    {
        public Task<EmployeeDTO?> ReadById(int id);
        public Task<List<EmployeeDTO>> ReadAll();
        public Task Create(EmployeeDTO employee);
        public Task<bool> Update(EmployeeDTO employee);
        public Task<bool> Delete(int id);
    }
}
