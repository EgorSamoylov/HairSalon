using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.EmployeeRepository
{
    public interface IEmployeeRepository
    {
        public Task<Employee?> ReadById(int id);
        public Task<List<Employee>> ReadAll();
        public Task Create(Employee employee);
        public Task<bool> Update(Employee employee);
        public Task<bool> Delete(int id);
    }
}
