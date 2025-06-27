using Application.DTOs;
using Application.Request.ClientRequest;

namespace Application.Services
{
    public interface IUserService
    {
        public Task<UserDTO> GetById(int id);
        public Task<IEnumerable<UserDTO>> GetAll();
        public Task<int> Add(CreateUserRequest user);
        public Task Update(UpdateUserRequest user);
        public Task Delete(int id);
        Task<string?> GetUserRoleById(int id);
    }
}
