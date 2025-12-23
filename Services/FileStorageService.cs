using Median.Core.Models.Common;
using Median.Intranet.DAL.Contracts;
using Median.Intranet.Models;
using System.Net.Mime;
using System.Security.Cryptography;

namespace Median.Intranet.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IDocumentEntityRepository _repo;
        private readonly string _rootPath;

        public FileStorageService(IWebHostEnvironment env, IDocumentEntityRepository repo)
        {
            _repo = repo;
            _rootPath = Path.Combine(env.ContentRootPath, "data", "uploads");
            Directory.CreateDirectory(_rootPath);
        }

        public async Task<Result<DocumentEntity>> SaveFileAsync(IFormFile file, Guid documentId, string name, string description, int version = 1)
        {
            try
            {
                var filePath = GetFilePath(documentId, file.FileName);

                // Gem filen
                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Opret dokument-entity
                var doc = new DocumentEntity
                {
                    Id = documentId,
                    FileName = file.Name,
                    ContentType = file.ContentType,
                    FilePath = filePath,
                    FileSize = file.Length,
                    Name = name,
                    Version = version,
                    Description = description,
                    CreatedAt = DateTime.UtcNow,
                };

                // Gem i databasen
                var saveddocument = await _repo.CreateAsync(doc);
                if (saveddocument.Success)
                {
                    return Result.Ok(saveddocument.Value);
                }
                else
                {
                    return Result.Fail<DocumentEntity>(saveddocument.Error);
                }
            }
            catch (Exception ex)
            {
                return Result.Fail<DocumentEntity>(Errors.General.FromException(ex));
            }
        }

        private string GetFilePath(Guid documentId, string fileName)
        {
            var folder = Path.Combine(_rootPath, documentId.ToString());
            Directory.CreateDirectory(folder);

            return Path.Combine(folder, fileName);
        }
    }
}
