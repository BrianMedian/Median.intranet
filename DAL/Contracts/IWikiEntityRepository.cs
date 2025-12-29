using Median.Core.Models.Common;
using Median.Intranet.Models;

namespace Median.Intranet.DAL.Contracts
{
    public interface IWikiEntityRepository
    {
        Task<Result<WikiEntity>> CreateAsync(WikiEntity entity);
        Task<Result<List<WikiEntity>>> GetAllAsync();
        Task<Result<WikiEntity>> GetByIdAsync(Guid id);        
        Task<Result> DeleteAsync(Guid documentId);
        Task<Result> UpdateAsync(WikiEntity entity);
        Task<Result<List<WikiEntity>>> GetAllRootAsync();
    }
}
