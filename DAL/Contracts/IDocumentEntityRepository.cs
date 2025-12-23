using Median.Core.Models.Common;
using Median.Intranet.Models;

namespace Median.Intranet.DAL.Contracts
{
    public interface IDocumentEntityRepository
    {
        Task<Result<DocumentEntity>> CreateAsync(DocumentEntity document);
        Task<Result<List<DocumentEntity>>> GetAllAsync();
        Task<Result<DocumentEntity>> GetByIdAsync(Guid id);        
        Task<Result> DeleteDocument(Guid documentId);
        Task<Result> UpdateDocument(DocumentEntity document);
    }
}
