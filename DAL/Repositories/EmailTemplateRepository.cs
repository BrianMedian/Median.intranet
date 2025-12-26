using Dapper;
using Median.Authentication.Simple.Common;
using Median.Core.Models.Common;
using Median.Intranet.DAL.Contracts;
using Median.Intranet.Models;
using Median.Intranet.Models.Emails;
using Microsoft.Extensions.Options;

namespace Median.Intranet.DAL.Repositories
{
    public class EmailTemplateRepository : BaseRepository, IEmailTemplateRepository
    {
        public EmailTemplateRepository(IOptions<DatabaseSettings> dbSettings) : base(dbSettings)
        {
        }

        public async Task<Result<EmailTemplate>> CreateAsync(EmailTemplate template)
        {
            try
            {
                const string sql = "insert into emailtemplates(name, subject, description, version, content, key) values (@name, @subject, @description, @version, @content, @key) RETURNING *";
                using var conn = CreateConnection();
                var newProduct = await conn.QuerySingleOrDefaultAsync<EmailTemplate>(sql, template);
                return Result<ProductEntity>.Ok(newProduct!);
            }
            catch (Exception ex)
            {
                return Result.Fail<EmailTemplate>(Errors.Database.CouldNotInsert(ex.Message));
            }
        }

        public async Task<Result> DeleteAsync(Guid templateId)
        {
            try
            {
                const string sql = "delete from emailtemplates where id = @id";
                using var conn = CreateConnection();
                await conn.ExecuteAsync(sql, new {id = templateId});
                return Result<EmailTemplate>.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail<EmailTemplate>(Errors.Database.CouldNotDelete(ex.Message));
            }
        }

        public async Task<Result<List<EmailTemplate>>> GetAllAsync()
        {
            try
            {
                const string sql = "select * from emailtemplates";
                using var conn = CreateConnection();
                var result = await conn.QueryAsync<EmailTemplate>(sql);
                return Result<EmailTemplate>.Ok(result.ToList());
            }
            catch (Exception ex)
            {
                return Result.Fail<List<EmailTemplate>>(Errors.Database.QueryError(ex.Message));
            }
        }

        public async Task<Result<EmailTemplate>> GetByIdAsync(Guid id)
        {
            try
            {
                const string sql = "select * from emailtemplates where id = @id";
                using var conn = CreateConnection();
                var result = await conn.QuerySingleOrDefaultAsync<EmailTemplate>(sql, new {id = id});                
                if(result == null)
                {
                    return Result.Fail<EmailTemplate>(Errors.Database.QueryError($"Could not find entity with id: {id}"));
                }
                return Result<EmailTemplate>.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail<EmailTemplate>(Errors.Database.QueryError(ex.Message));
            }
        }

        public async Task<Result> UpdateAsync(EmailTemplate template)
        {
            try
            {
                template.UpdatedAt = DateTime.UtcNow;
                const string sql = "update emailtemplates set name = @name, subject = @subject, description = @description, version=@version, content=@content, updatedat=@updatedat where id = @id";
                using var conn = CreateConnection();
                await conn.ExecuteAsync(sql, template);
                return Result<ProductEntity>.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail<EmailTemplate>(Errors.Database.CouldNotUpdate(ex.Message));
            }
        }
    }
}
