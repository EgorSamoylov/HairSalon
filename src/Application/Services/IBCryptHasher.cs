namespace Application.Services
{
    public interface IBCryptHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string? storedHash);
    }
}
