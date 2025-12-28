using Median.Core.Models.Common;

namespace Median.Intranet.Services
{
    public interface IEmailService
    {
        Task<Result<bool>> SendFileToRecipient(string toEmail, Guid fileId, Guid sendingUserId);
        Task<Result<bool>> SendBusinessCardToRecipient(string toEmail, Guid sendingUserId);
    }
}
