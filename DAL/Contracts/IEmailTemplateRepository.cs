using Median.Core.Models.Common;
using Median.Intranet.Models;
using Median.Intranet.Models.Emails;

namespace Median.Intranet.DAL.Contracts
{
    public interface IEmailTemplateRepository
    {
        Task<Result<EmailTemplate>> CreateAsync(EmailTemplate template);
        Task<Result<List<EmailTemplate>>> GetAllAsync();
        Task<Result<EmailTemplate>> GetByIdAsync(Guid id);
        Task<Result> DeleteAsync(Guid documentId);
        Task<Result> UpdateAsync(EmailTemplate template);
    }
}
