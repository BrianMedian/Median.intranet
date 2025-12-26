using Dapper;
using Median.Authentication.Simple.Common;
using Median.Core.Models.Common;
using Median.Intranet.DAL.Contracts;
using Median.Intranet.Models;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata;

namespace Median.Intranet.DAL.Repositories
{
    public class ProductEntityRepository : BaseRepository, IProductEntityRepository
    {
        public ProductEntityRepository(IOptions<DatabaseSettings> dbSettings) : base(dbSettings)
        {
        }

        public async Task<Result<ProductEntity>> CreateAsync(ProductEntity product)
        {
            try
            {
                const string sql = "insert into products(name, description, price) values (@name, @description, @price) RETURNING *";
                using var conn = CreateConnection();
                var newProduct = await conn.QuerySingleOrDefaultAsync<ProductEntity>(sql, product);
                return Result<ProductEntity>.Ok(newProduct!);
            }
            catch (Exception ex)
            {
                return Result.Fail<ProductEntity>(Errors.Database.CouldNotInsert(ex.Message));
            }
        }

        public async Task<Result> DeleteAsync(Guid documentId)
        {
            try
            {
                const string sql = "delete from products where id = @id";
                using var conn = CreateConnection();
                await conn.ExecuteAsync(sql, new {id = documentId});
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(Errors.Database.CouldNotDelete(ex.Message));
            }
        }

        public async Task<Result<List<ProductEntity>>> GetAllAsync()
        {
            try
            {
                const string sql = "select * from products order by name";
                using var conn = CreateConnection();
                var result = await conn.QueryAsync<ProductEntity>(sql);
                return Result.Ok(result.ToList());
            }
            catch (Exception ex)
            {
                return Result.Fail<List<ProductEntity>>(Errors.Database.QueryError(ex.Message));
            }
        }

        public async Task<Result<ProductEntity>> GetByIdAsync(Guid id)
        {
            try
            {
                const string sql = "select * from products where id = @id";
                using var conn = CreateConnection();
                var result = await conn.QuerySingleOrDefaultAsync<ProductEntity>(sql, new {id = id});
                return Result.Ok(result!);
            }
            catch (Exception ex)
            {
                return Result.Fail<ProductEntity>(Errors.Database.QueryError(ex.Message));
            }
        }

        public async Task<Result> UpdateAsync(ProductEntity product)
        {
            try
            {
                const string sql = "update products set name = @name, description = @description, price = @price where id = @id";
                using var conn = CreateConnection();
                var newProduct = await conn.ExecuteAsync(sql, new {name = product.Name, description = product.Description, price = product.Price});
                return Result<ProductEntity>.Ok(newProduct!);
            }
            catch (Exception ex)
            {
                return Result.Fail<ProductEntity>(Errors.Database.CouldNotUpdate(ex.Message));
            }
        }
    }
}
