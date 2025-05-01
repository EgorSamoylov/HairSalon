using Domain.Entities;
using Infrastructure.Repositories.AttachmentRepository;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    internal class AttachmentService(
         IAttachmentRepository repository,
         IFileStorageService fileStorage,
         IHttpContextAccessor httpContextAccessor)
         : IAttachmentService
    {
        public async Task<Attachment> UploadAsync(IFormFile file, string category)
        {
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var storedPath = await fileStorage.SaveFile(file, category, fileName);

            var attachment = new Attachment
            {
                FileName = file.FileName,
                StoredPath = storedPath,
                ContentType = file.ContentType,
                Size = file.Length,
                CreatedAt = DateTime.UtcNow
            };

            await repository.Save(attachment);
            return attachment;
        }

        public async Task<byte[]> GetFileContentAsync(int id)
        {
            var attachment = await repository.Get(id);
            if (attachment == null || !fileStorage.FileExists(attachment.StoredPath))
                throw new FileNotFoundException("Attachment not found");

            return await fileStorage.ReadFile(attachment.StoredPath);
        }

        public async Task DeleteAsync(int id)
        {
            var attachment = await repository.Get(id);
            if (attachment == null)
                return;

            fileStorage.DeleteFile(attachment.StoredPath);
            await repository.Delete(id);
        }

        public async Task<string> GetPublicLinkAsync(int id)
        {
            var context = httpContextAccessor.HttpContext;
            var request = context.Request;

            var attachment = await repository.Get(id);
            if (attachment == null)
                throw new FileNotFoundException("Attachment not found");

            var baseUrl = $"{request.Scheme}://{request.Host}";
            return $"{baseUrl}/api/attachments/{id}/download";
        }

        public async Task<Attachment?> GetMetadataAsync(int id)
        {
            return await repository.Get(id);
        }
    }
}
