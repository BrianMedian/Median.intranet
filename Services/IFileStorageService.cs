using Median.Core.Models.Common;
using Median.Intranet.Models;

namespace Median.Intranet.Services
{
    public interface IFileStorageService
    {
        Task<Result<DocumentEntity>> SaveFileAsync(IFormFile file, Guid documentId);
        Task<Result<FileStream>> GetFileAsync(Guid id);
        //Task<bool> DeleteFileAsync(Guid id);
    }
}
