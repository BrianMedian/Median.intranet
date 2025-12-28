
using Median.Core.Models.Common;
using Median.Intranet.Models.Emails;

namespace Median.Intranet.DAL.Contracts
{
    public interface IEmailLogRepository
    {
        Task<Result> CreateLogAsync(EmailLog emailLog);
        Task<Result> DeleteLogAsync(Guid id);
        Task<Result<List<EmailLog>>> GetAllAsync();
        Task<Result<EmailLog>> GetByIdAsync(int id);
        Task<Result> DeleteAllSinceDaysAsync(int days);//negative number that denotes the amount of days.
    }
}
