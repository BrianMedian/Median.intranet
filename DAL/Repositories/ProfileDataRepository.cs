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
    public class ProfileDataRepository : BaseRepository, IProfileDataRepository
    {
        public ProfileDataRepository(IOptions<DatabaseSettings> dbSettings) : base(dbSettings)
        {
        }

        public async Task<Result<ProfileData>> CreateAsync(ProfileData entity)
        {
            try
            {
                const string sql = "insert into profiledata(userid, fullname, phonenumber, position, email) values (@userid, @fullname, @phonenumber, @position, @email) RETURNING *";
                using var conn = CreateConnection();
                var result = await conn.QuerySingleOrDefaultAsync<ProfileData>(sql, entity);
                return Result<ProfileData>.Ok(result!);
            }
            catch (Exception ex)
            {
                return Result.Fail<ProfileData>(Errors.Database.CouldNotInsert(ex.Message));
            }
        }

        public async Task<Result> DeleteAsync(Guid entityId)
        {
            try
            {
                const string sql = "delete from profiledata where id = @id";
                using var conn = CreateConnection();
                await conn.ExecuteAsync(sql, new { id = entityId });
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(Errors.Database.CouldNotDelete(ex.Message));
            }
        }

        public async Task<Result<List<ProfileData>>> GetAllAsync()
        {
            try
            {
                const string sql = "select * from profiledata";
                using var conn = CreateConnection();
                var result = await conn.QueryAsync<ProfileData>(sql);
                return Result<EmailTemplate>.Ok(result.ToList());
            }
            catch (Exception ex)
            {
                return Result.Fail<List<ProfileData>>(Errors.Database.QueryError(ex.Message));
            }
        }

        public async Task<Result<ProfileData>> GetByIdAsync(Guid id)
        {
            try
            {
                const string sql = "select * from profiledata where id = @id";
                using var conn = CreateConnection();
                var result = await conn.QuerySingleOrDefaultAsync<ProfileData>(sql, new { id = id });
                if (result == null)
                {
                    return Result.Fail<ProfileData>(Errors.Database.QueryError($"Could not find entity with id: {id}"));
                }
                return Result<ProfileData>.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail<ProfileData>(Errors.Database.QueryError(ex.Message));
            }
        }

        public async Task<Result<ProfileData>> GetByUserIdAsync(Guid id)
        {
            try
            {
                const string sql = "select * from profiledata where userid = @id";
                using var conn = CreateConnection();
                var result = await conn.QuerySingleOrDefaultAsync<ProfileData>(sql, new { id = id });
                if (result == null)
                {
                    return Result.Fail<ProfileData>(Errors.Database.QueryError($"Could not find entity with id: {id}"));
                }
                return Result<ProfileData>.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail<ProfileData>(Errors.Database.QueryError(ex.Message));
            }
        }

        public async Task<Result> UpdateAsync(ProfileData entity)
        {
            try
            {
                entity.UpdatedAt = DateTime.UtcNow;
                const string sql = "update profiledata set fullname = @fullname, phonenumber = @phonenumber, position = @position, email = @email, onlinecardurl = @onlinecardurl, profilepictureurl = @profilepictureurl where id = @id";
                using var conn = CreateConnection();
                await conn.ExecuteAsync(sql, entity);
                return Result<ProfileData>.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail<ProfileData>(Errors.Database.CouldNotUpdate(ex.Message));
            }
        }
    }
}
