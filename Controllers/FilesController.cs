using Median.Intranet.DAL.Contracts;
using Median.Intranet.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Median.Intranet.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : BaseController
    {
        private readonly IWebHostEnvironment _env;
        private readonly IDocumentEntityRepository _repo;        
        private readonly IFileStorageService _storage;        

        public FilesController(IWebHostEnvironment env,
            IDocumentEntityRepository repo,           
            IFileStorageService fileStorageService)
        {
            _env = env;
            _repo = repo;
            _storage = fileStorageService;
        }

        [HttpPost("upload")]
        [RequestSizeLimit(200_000_000)]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            //only pdf and word files allowed
            var allowed = new[]
            {
        "application/pdf",        
        "application/msword",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
    };

            if (!allowed.Contains(file.ContentType))
            {
                return BadRequest($"File type not allowed: {file.ContentType}");
            }

            var saved = await _storage.SaveFileAsync(file, Guid.NewGuid());                        

            return Ok(saved);
        }

        /// <summary>
        /// This will delete the file and all the chunks
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteFile(string id)
        {
            //id of the document
            Guid documentId = Guid.Parse(id);
            //then delete the document
            var result = await _repo.DeleteDocument(documentId);
            if (result.Success)
            {
                return Ok();
            }
            else
            {
                return Error(result.Error.Message);
            }                
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFiles()
        {
            var result = await this._repo.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadFile(string id)
        {
            if (!Guid.TryParse(id, out Guid fileId))
            {
                return BadRequest("Invalid file ID");
            }

            // Hent fil metadata
            var docResult = await _repo.GetByIdAsync(fileId);
            if (!docResult.Success)
            {
                return NotFound(docResult.Error.Message);
            }

            var doc = docResult.Value;

            // Hent file stream
            var fileResult = await _storage.GetFileAsync(fileId);
            if (!fileResult.Success)
            {
                return NotFound(fileResult.Error.Message);
            }

            // Returner filen
            return File(
                fileResult.Value,
                doc.ContentType ?? "application/octet-stream",
                doc.FileName
            );
        }

        [HttpGet("{fileid}")]
        public async Task<IActionResult> GetFile(string fileid)
        {
            Guid id = Guid.Parse(fileid);
            var fileResult = await this._repo.GetByIdAsync(id);
            if (fileResult.Success)
            {
                return Ok(fileResult.Value);
            }
            else
            {
                return Error(fileResult.Error.Message);
            }                
        }
    }

    public record UploadDocumentRequest(string name, string description);
}
