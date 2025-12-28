using Dapper;
using Median.Authentication.Simple.Common;
using Median.Authentication.Simple.Models;
using Median.Authentication.Simple.Repositories.Contract;
using Median.Core.Models.Common;
using Median.Intranet.DAL.Contracts;
using Median.Intranet.Models;
using Median.Intranet.Models.Dto;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata;

namespace Median.Intranet.DAL.Repositories
{
    public class AdminUserRepository : BaseRepository, IAdminUserRepository
    {
        public AdminUserRepository(IOptions<DatabaseSettings> dbSettings) : base(dbSettings)
        {
        }

        public async Task<Result<List<UserDto>>> GetAllUsersAsync()
        {
            try
            {
                const string sql = "select users.id as userid, users.email as useremail, users.userrole, profiledata.fullname as name from users join profiledata on users.id = profiledata.userid";
                using var conn = CreateConnection();
                var results = await conn.QueryAsync<UserDto>(sql);
                return Result.Ok<List<UserDto>>(results.ToList());
            }
            catch (Exception ex)
            {
                return Result.Fail<List<UserDto>>(Errors.Database.QueryError(ex.Message));
            }
        }

        public async Task<Result<UserDto>> GetUser(Guid id)
        {
            try
            {
                const string sql = "select users.id as userid, users.email as useremail, users.userrole, profiledata.fullname as name from users join profiledata on users.id = profiledata.userid where users.id = @id";
                using var conn = CreateConnection();
                var result = await conn.QuerySingleOrDefaultAsync<UserDto>(sql, new {id = id});
                return Result.Ok<UserDto>(result);
            }
            catch (Exception ex)
            {
                return Result.Fail<UserDto>(Errors.Database.QueryError(ex.Message));
            }
        }
    }
}
