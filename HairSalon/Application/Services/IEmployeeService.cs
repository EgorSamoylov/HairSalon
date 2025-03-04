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
        public Task<List<EmployeeDTO>> GetAll();
        public Task Add(EmployeeDTO employee);
        public Task<bool> Update(EmployeeDTO employee);
        public Task<bool> Delete(int id);
    }
}
