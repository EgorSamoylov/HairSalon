using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public interface IAttachmentService
    {
        Task<Attachment> UploadAsync(IFormFile file, string category);
        Task<byte[]> GetFileContentAsync(int id);
        Task DeleteAsync(int id);
        Task<string> GetPublicLinkAsync(int id);
        Task<Attachment?> GetMetadataAsync(int id);
    }
}
