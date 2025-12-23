using Median.Core.Models.Common;
using Median.Intranet.Models;

namespace Median.Intranet.DAL.Contracts
{
    public interface IProductEntityRepository
    {
        Task<Result<ProductEntity>> CreateAsync(ProductEntity document);
        Task<Result<List<ProductEntity>>> GetAllAsync();
        Task<Result<ProductEntity>> GetByIdAsync(Guid id);
        Task<Result> DeleteAsync(Guid documentId);
        Task<Result> UpdateAsync(ProductEntity document);
    }
}
