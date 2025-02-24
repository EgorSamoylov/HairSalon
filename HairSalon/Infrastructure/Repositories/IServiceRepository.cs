using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IServiceRepository
    {
        public Task<Service?> ReadById(int id);
        public Task<List<Service>> ReadAll();
        public Task Create(Service service);
        public Task<bool> Update(Service service);
        public Task<bool> Delete(int id);
    }
}
