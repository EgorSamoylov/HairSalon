using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IClientService
    {
        public Task<ClientDTO?> GetById(int id);
        public Task<List<ClientDTO>> GetAll();
        public Task Add(ClientDTO client);
        public Task<bool> Update(ClientDTO client);
        public Task<bool> Delete(int id);
    }
}
