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
        public EmailTypeRepository(IOptions<DatabaseSettings> dbSettings) : base(dbSettings)
        {
            EnsureAll();
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
                return Result.Fail<List<EmailTypeDto>>(Errors.Database.GeneralError(ex.Message));
            }
        }

        public async Task<Result<string>> GetByEmailType(string emailType)
        {
            try
            {
                const string sql = "select templateid from emailsettings where emailtype = @type";
                using var conn = CreateConnection();
                var result = await conn.QuerySingleOrDefaultAsync<string>(sql, new { type = emailType });
                if (!string.IsNullOrEmpty(result))
                {
                    return Result<string>.Ok(result);
                }
                return Result.Fail<string>(Errors.Database.QueryError("Email type not found"));
            }
            catch (Exception ex)
            {
                return Result.Fail<string>(Errors.Database.GeneralError(ex.Message));
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
                //log it
            }
        }
    }
}
