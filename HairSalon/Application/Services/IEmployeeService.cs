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
        public Task<EmployeeDTO?> GetById(int id);
        public Task<IEnumerable<EmployeeDTO>> GetAll();
        public Task<int> Add(EmployeeDTO employee);
        public Task<bool> Update(EmployeeDTO employee);
        public Task<bool> Delete(int id);
    }
}
