using Application.DTOs;
using Application.Request.ClientRequest;

namespace Application.Services
{
    public interface IClientService
    {
        public Task<ClientDTO?> GetById(int id);
        public Task<IEnumerable<ClientDTO>> GetAll();
        public Task Add(CreateClientRequest client);
        public Task<bool> Update(UpdateClientRequest client);
        public Task<bool> Delete(int id);
    }
}
