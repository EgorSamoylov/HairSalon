using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public interface IFileStorageService
    {
        Task<string> SaveFile(IFormFile file, string subFolder, string fileName);
        bool FileExists(string path);
        void DeleteFile(string path);
        Task<byte[]> ReadFile(string path);
        string GetFilePublicPath(string storedPath);
    }
}
