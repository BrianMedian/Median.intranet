using Dapper;
using Median.Authentication.Simple.Common;
using Median.Core.Models.Common;
using Median.Intranet.DAL.Contracts;
using Median.Intranet.Models;
using Microsoft.Extensions.Options;
using static Dapper.SqlMapper;

namespace Median.Intranet.DAL.Repositories
{
    public class WikiEntityRepository : BaseRepository, IWikiEntityRepository
    {
        protected WikiEntityRepository(IOptions<DatabaseSettings> dbSettings) : base(dbSettings)
        {
        }

        public async Task<Result<WikiEntity>> CreateAsync(WikiEntity entity)
        {
            try
            {
                const string sql = "insert into wikis(title, markdown, parentid, sortorder, isactive) values (@title, @markdown, @parentid, @sortorder, @isactive) RETURNING *";
                using var conn = CreateConnection();
                var result = await conn.QuerySingleOrDefaultAsync<WikiEntity>(sql, entity);
                return Result<WikiEntity>.Ok(result!);
            }
            catch (Exception ex)
            {
                return Result.Fail<WikiEntity>(Errors.Database.CouldNotInsert(ex.Message));
            }
        }

        public async Task<Result> DeleteAsync(Guid entityId)
        {
            try
            {
                const string sql = "delete from wikis where id = @id";
                using var conn = CreateConnection();
                await conn.ExecuteAsync(sql, new {id = entityId});
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(Errors.Database.CouldNotDelete(ex.Message));
            }
        }

        public async Task<Result<List<WikiEntity>>> GetAllAsync()
        {
            try
            {
                const string sql = "select * from wikis";
                using var conn = CreateConnection();
                var result = await conn.QueryAsync<WikiEntity>(sql);
                return Result<List<WikiEntity>>.Ok(result.ToList());
            }
            catch (Exception ex)
            {
                return Result.Fail<List<WikiEntity>>(Errors.Database.QueryError(ex.Message));
            }
        }

        public async Task<Result<List<WikiEntity>>> GetAllRootAsync()
        {
            try
            {
                const string sql = "select * from wikis where parentid = @id";
                using var conn = CreateConnection();
                var result = await conn.QueryAsync<WikiEntity>(sql, new {id = Guid.Empty});
                return Result<List<WikiEntity>>.Ok(result.ToList());
            }
            catch (Exception ex)
            {
                return Result.Fail<List<WikiEntity>>(Errors.Database.QueryError(ex.Message));
            }
        }

        public async Task<Result<WikiEntity>> GetByIdAsync(Guid id)
        {
            try
            {
                const string sql = "select * from wikis where id = @id";
                using var conn = CreateConnection();
                var result = await conn.QuerySingleOrDefaultAsync<WikiEntity>(sql, new {id = id});
                return Result<WikiEntity>.Ok(result!);
            }
            catch (Exception ex)
            {
                return Result.Fail<WikiEntity>(Errors.Database.QueryError(ex.Message));
            }
        }

        public async Task<Result> UpdateAsync(WikiEntity entity)
        {
            try
            {
                const string sql = "update wikis set title = @title, markdown = @markdown, parentid = @parentid, sortorder = @sortorder, isactive = @isactive where id = @id";
                using var conn = CreateConnection();
                await conn.ExecuteAsync(sql, entity);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(Errors.Database.CouldNotUpdate(ex.Message));
            }
        }
    }
}
