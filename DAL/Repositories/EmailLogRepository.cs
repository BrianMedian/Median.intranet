using Dapper;
using Median.Authentication.Simple.Common;
using Median.Core.Models.Common;
using Median.Intranet.DAL.Contracts;
using Median.Intranet.Models;
using Median.Intranet.Models.Emails;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata;

namespace Median.Intranet.DAL.Repositories
{
    public class EmailLogRepository : BaseRepository, IEmailLogRepository
    {
        private readonly ILogger<EmailLogRepository> logger;
        public EmailLogRepository(IOptions<DatabaseSettings> dbSettings, ILogger<EmailLogRepository> logger) : base(dbSettings)
        {
            this.logger = logger;
        }

        public async Task<Result> CreateLogAsync(EmailLog emailLog)
        {
            try
            {
                const string sql = "insert into emaillog (userid, emailtypeid, fromemail, toemail, templateid, status) values (@userid, @emailtypeid, @fromemail, @toemail, @templateid, @status) RETURNING *";
                using var conn = CreateConnection();
                await conn.ExecuteAsync(sql, emailLog);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                this.logger.LogCritical(ex, ex.Message);
                return Result.Fail(Errors.Database.CouldNotInsert(ex.Message));
            }
        }

        public async Task<Result> DeleteAllSinceDaysAsync(int days)
        {
            try
            {
                const string sql = "delete from emaillog where createdAt < @cutoffdate";
                using var conn = CreateConnection();
                await conn.ExecuteAsync(sql, new {cutoffdate = DateTime.UtcNow.AddDays(days)});
                return Result.Ok();
            }
            catch (Exception ex)
            {
                this.logger.LogCritical(ex, ex.Message);
                return Result.Fail(Errors.Database.CouldNotDelete(ex.Message));
            }
        }

        public async Task<Result> DeleteLogAsync(Guid id)
        {
            try
            {
                const string sql = "delete from emaillog where id = @id";
                using var conn = CreateConnection();
                await conn.ExecuteAsync(sql, new {id = id});
                return Result.Ok();
            }
            catch (Exception ex)
            {
                this.logger.LogCritical(ex, ex.Message);
                return Result.Fail(Errors.Database.CouldNotDelete(ex.Message));
            }
        }

        public async Task<Result<List<EmailLog>>> GetAllAsync()
        {
            try
            {
                const string sql = "select * from emaillog order by createdAt";
                using var conn = CreateConnection();
                var logs = await conn.QueryAsync<EmailLog>(sql);
                return Result.Ok(logs.ToList());
            }
            catch (Exception ex)
            {
                this.logger.LogCritical(ex, ex.Message);
                return Result.Fail<List<EmailLog>>(Errors.Database.QueryError(ex.Message));
            }
        }

        public async Task<Result<EmailLog>> GetByIdAsync(int id)
        {
            try
            {
                const string sql = "select * from emaillog where id = @id";
                using var conn = CreateConnection();
                var log = await conn.QuerySingleOrDefaultAsync<EmailLog>(sql, new {id = id});
                if(log == null)
                {
                    return Result.Fail<EmailLog>(Errors.Database.QueryError($"Could not find {id}"));
                }
                return Result.Ok(log);
            }
            catch (Exception ex)
            {
                this.logger.LogCritical(ex, ex.Message);
                return Result.Fail<EmailLog>(Errors.Database.CouldNotInsert(ex.Message));
            }
        }
    }
}
