using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IServiceService
    {
        public Task<ServiceDTO?> GetById(int id);
        public Task<List<ServiceDTO>> GetAll();
        public Task Add(ServiceDTO service);
        public Task<bool> Update(ServiceDTO service);
        public Task<bool> Delete(int id);
    }
}
