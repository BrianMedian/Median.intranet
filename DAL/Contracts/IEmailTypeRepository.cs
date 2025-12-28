using Median.Core.Models.Common;
using Median.Intranet.Models.Emails;

namespace Median.Intranet.DAL.Contracts
{
    public interface IEmailTypeRepository
    {
        Task<Result<List<EmailTypeDto>>> GetAllAsync();
        Task<Result<EmailTypeDto>> GetByEmailType(string id);
        Task<Result> Update(string emailtype, Guid templateid);
    }
}
