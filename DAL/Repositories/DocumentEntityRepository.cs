using Dapper;
using Median.Authentication.Simple.Common;
using Median.Core.Models.Common;
using Median.Intranet.DAL.Contracts;
using Median.Intranet.Models;
using Microsoft.Extensions.Options;

namespace Median.Intranet.DAL.Repositories
{
    public class DocumentEntityRepository : BaseRepository, IDocumentEntityRepository
    {
        public DocumentEntityRepository(IOptions<DatabaseSettings> dbSettings) : base(dbSettings)
        {
        }

        public async Task<Result<DocumentEntity>> CreateAsync(DocumentEntity document)
        {
            try
            {
                const string sql = "insert into documents(name, contenttype, description, version, filepath, filename, filesize) values (@name, @contenttype, @description, @version, @filepath, @filename, @filesize) RETURNING *";
                using var conn = CreateConnection();
                var newDocument = await conn.QuerySingleOrDefaultAsync<DocumentEntity>(sql, document);
                return Result<DocumentEntity>.Ok(newDocument!);
            }
            catch (Exception ex)
            {
                return Result.Fail<DocumentEntity>(Errors.Database.CouldNotInsert(ex.Message));
            }
        }

        public async Task<Result> DeleteDocument(Guid documentId)
        {
            try
            {
                const string sql = "delete from documents where id = @id";
                using var conn = CreateConnection();
                await conn.ExecuteAsync(sql, new { id = documentId });
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(Errors.Database.CouldNotDelete(ex.Message));
            }
        }

        public async Task<Result<List<DocumentEntity>>> GetAllAsync()
        {
            try
            {
                const string sql = "select * from documents order by name";
                using var conn = CreateConnection();
                var result = await conn.QueryAsync<DocumentEntity>(sql);
                return Result.Ok(result.ToList());
            }
            catch (Exception ex)
            {
                return Result.Fail<List<DocumentEntity>>(Errors.Database.QueryError(ex.Message));
            }
        }

        public async Task<Result<DocumentEntity>> GetByIdAsync(Guid id)
        {
            try
            {
                const string sql = "select * from documents where id = @id";
                using var conn = CreateConnection();
                var result = await conn.QuerySingleOrDefaultAsync<DocumentEntity>(sql, new { id=id });
                return Result.Ok<DocumentEntity>(result!);
            }
            catch (Exception ex)
            {
                return Result.Fail<DocumentEntity>(Errors.Database.CouldNotDelete(ex.Message));
            }
        }

        public async Task<Result> UpdateDocument(DocumentEntity document)
        {
            try
            {
                const string sql = "update documents set name = @name, contenttype = @contenttype, description = @description, version = @version, filepath = @filepath, filename = @filename, filesize = @filesize where id = @id";
                using var conn = CreateConnection();
                await conn.ExecuteAsync(sql, document);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(Errors.Database.CouldNotUpdate(ex.Message));
            }
        }
    }
}
