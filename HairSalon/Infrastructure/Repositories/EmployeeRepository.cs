using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private List<Employee> employees = new List<Employee>();
        public EmployeeRepository() { }
        public Task Create(Employee employee)
        {
            employees.Add(employee);
            return Task.CompletedTask;
        }

        public Task<bool> Delete(int id)
        {
            if (!employees.Any(x => x.Id == id))
            {
                return Task.FromResult(false);
            }

            employees.RemoveAll(x => x.Id == id);

            return Task.FromResult(true);
        }

        public Task<List<Employee>> ReadAll()
        {
            return Task.FromResult(employees);
        }

        public Task<Employee?> ReadById(int id)
        {
            var employee = employees.Find(x => x.Id == id);
            return Task.FromResult(employee);
        }

        public Task<bool> Update(Employee employee)
        {
            var employeeToUpdate = employees.Find(x => x.Id == employee.Id);

            if (employeeToUpdate != null)
            {
                return Task.FromResult(false);
            }

            employeeToUpdate.FirstName = employee.FirstName;
            employeeToUpdate.LastName = employee.LastName;
            employeeToUpdate.PhoneNumber = employee.PhoneNumber;
            employeeToUpdate.Email = employee.Email;
            employeeToUpdate.Position = employee.Position;

            return Task.FromResult(true);

        }
    }
}
