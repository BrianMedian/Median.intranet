using Median.Core.Models.Common;
using Median.Intranet.Models;
using Median.Intranet.Models.Emails;

namespace Median.Intranet.DAL.Contracts
{
    public interface IProfileDataRepository
    {
        Task<Result<ProfileData>> CreateAsync(ProfileData entity);
        Task<Result<List<ProfileData>>> GetAllAsync();
        Task<Result<ProfileData>> GetByIdAsync(Guid id);
        Task<Result<ProfileData>> GetByUserIdAsync(Guid userId);
        Task<Result> DeleteAsync(Guid documentId);
        Task<Result> UpdateAsync(ProfileData entity);
    }
}
