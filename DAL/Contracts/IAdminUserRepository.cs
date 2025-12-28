using Median.Core.Models.Common;
using Median.Intranet.Models.Dto;

namespace Median.Intranet.DAL.Contracts
{
    public interface IAdminUserRepository
    {
        Task<Result<List<UserDto>>> GetAllUsersAsync();
        Task<Result<UserDto>> GetUser(Guid id);
    }
}
