using Median.Core.Models.Common;
using Median.Intranet.Models;

namespace Median.Intranet.Services
{
    public interface IFileStorageService
    {
        Task<Result<DocumentEntity>> SaveFileAsync(IFormFile file, Guid documentId, string name, string description, int version = 1);
    }
}
