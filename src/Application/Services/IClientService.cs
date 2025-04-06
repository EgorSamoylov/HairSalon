using Application.DTOs;
using Application.Request.ClientRequest;

namespace Application.Services
{
    public interface IClientService
    {
        public Task<ClientDTO?> GetById(int id);
        public Task<IEnumerable<ClientDTO>> GetAll();
        public Task<int> Add(CreateClientRequest client);
        public Task Update(UpdateClientRequest client);
        public Task Delete(int id);
    }
}
