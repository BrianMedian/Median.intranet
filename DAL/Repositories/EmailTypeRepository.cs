using Dapper;
using Median.Authentication.Simple.Common;
using Median.Core.Models.Common;
using Median.Intranet.DAL.Contracts;
using Median.Intranet.Models;
using Median.Intranet.Models.Emails;
using Microsoft.Extensions.Options;

namespace Median.Intranet.DAL.Repositories
{
    public class EmailTypeRepository : BaseRepository, IEmailTypeRepository
    {
        private readonly ILogger<EmailTypeRepository> logger;
        public EmailTypeRepository(IOptions<DatabaseSettings> dbSettings, ILogger<EmailTypeRepository> logger) : base(dbSettings)
        {
            EnsureAll();
            this.logger = logger;
        }

        public async Task<Result<List<EmailTypeDto>>> GetAllAsync()
        {
            try
            {
                const string sql = "select * from emailsettings";
                using var conn = CreateConnection();
                var result = await conn.QueryAsync<EmailTypeDto>(sql);
               return Result.Ok(result.ToList());
            }
            catch (Exception ex)
            {
                this.logger.LogCritical(ex, ex.Message);
                return Result.Fail<List<EmailTypeDto>>(Errors.Database.GeneralError(ex.Message));
            }
        }

        public async Task<Result<EmailTypeDto>> GetByEmailType(string emailType)
        {
            try
            {
                const string sql = "select * from emailsettings where emailtype = @type";
                using var conn = CreateConnection();
                var result = await conn.QuerySingleOrDefaultAsync<EmailTypeDto>(sql, new { type = emailType });
                if(result != null)
                {
                    return Result.Ok<EmailTypeDto>(result);
                } 
                return Result.Fail<EmailTypeDto>(Errors.Database.QueryError($"Email type not found: {emailType}"));
            }
            catch (Exception ex)
            {
                this.logger.LogCritical(ex, ex.Message);
                return Result.Fail<EmailTypeDto>(Errors.Database.GeneralError(ex.Message));
            }
        }

        private void EnsureAll()
        {
            var allTypes = EmailTypes.All;
            foreach (var emailType in allTypes)
            {
                EnsureEmailType(emailType).GetAwaiter().GetResult();
            }
        }

        public async Task<Result> Update(string emailtype, Guid templateid)
        {
            try
            {
                const string sql = "update emailsettings set templateid = @id where emailtype = @type";
                using var conn = CreateConnection();
                await conn.ExecuteAsync(sql, new { id = templateid, type = emailtype });
                return Result.Ok();
            }
            catch (Exception ex)
            {
                this.logger.LogCritical(ex, ex.Message);
                return Result.Fail(Errors.Database.GeneralError(ex.Message));
            }
        }

        private async Task EnsureEmailType(string emailtype)
        {
            try
            {
                const string sql = "select count(*) from emailsettings where emailtype = @type";
                using var conn = CreateConnection();
                var result = await conn.ExecuteScalarAsync<int>(sql, new { type = emailtype });
                if (result <= 0)
                {
                    const string createSql = "insert into emailsettings(emailtype) values (@type)";
                    await conn.ExecuteAsync(createSql, new { type = emailtype });
                }
            }
            catch (Exception ex)
            {
                this.logger.LogCritical(ex, ex.Message);
            }
        }
    }
}
