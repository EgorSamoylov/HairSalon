using Dapper;
using Domain.Entities;
using Npgsql;

namespace Infrastructure.Repositories.AttachmentRepository
{
    public class AttachmentPostgresRepository(NpgsqlConnection connection) : IAttachmentRepository
    {
        public async Task Delete(int id)
        {
            var sql = "DELETE FROM attachments WHERE id = @id";
            await connection.ExecuteAsync(sql, new { id });
        }

        public async Task<Attachment?> Get(int id)
        {
            var sql = "SELECT * FROM attachments WHERE id = @id";
            return await connection.QueryFirstOrDefaultAsync<Attachment>(sql, new { id });
        }

        public async Task<int> Save(Attachment attachment)
        {
            var sql = @"
             INSERT INTO attachments (file_name, stored_path, content_type, size, created_at)
             VALUES (@FileName, @StoredPath, @ContentType, @Size, @CreatedAt)
             RETURNING id";

            var id = await connection.ExecuteScalarAsync<int>(sql, attachment);
            attachment.Id = id;
            return id;
        }
    }
}
