using Domain.Entities;

namespace Infrastructure.Repositories.ClientRepository
{
    public interface IUserRepository
    {
        public Task<User?> ReadById(int id);
        public Task<IEnumerable<User>> ReadAll();
        public Task<int> Create(User user);
        public Task<bool> Update(User user);
        Task<User?> ReadByEmail(string email);
        public Task<bool> Delete(int id);
        Task<string?> GetUserRoleById(int id);
    }
}
