using Median.Intranet.DAL.Contracts;
using Median.Intranet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Median.Intranet.Controllers
{
    [Route("api/wikis")]
    [ApiController]
    public class WikiController : BaseController
    {
        private readonly IWikiEntityRepository wikiRepository;
        private readonly ILogger<WikiController> logger;
        public WikiController(IWikiEntityRepository wikiRepository, ILogger<WikiController> logger)
        {
            this.wikiRepository = wikiRepository;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWikis()
        {
            var result = await this.wikiRepository.GetAllAsync();
            return FromResult(result);
        }

        [HttpGet("root")]
        public async Task<IActionResult> GetAllRootWikis()
        {
            var result = await this.wikiRepository.GetAllRootAsync();
            return FromResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWikiById(string id)
        {
            var result = await this.wikiRepository.GetByIdAsync(Guid.Parse(id));
            return FromResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWiki(CreateWikiRequest request)
        {
            WikiEntity entity = new WikiEntity();
            entity.Title = request.Title;
            entity.IsActive = true;
            entity.Markdown = request.Markdown ?? string.Empty;
            entity.ParentId = request.ParentId == null ? Guid.Empty : Guid.Parse(request.ParentId);
            var result = await this.wikiRepository.CreateAsync(entity);
            return FromResult(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateWiki(UpdateWikiRequest request)
        {
            WikiEntity entity = new WikiEntity();
            entity.Id = Guid.Parse(request.Id);
            entity.Title = request.Title;
            entity.IsActive = true;
            entity.Markdown = request.Markdown ?? string.Empty;
            entity.ParentId = request.ParentId == null ? Guid.Empty : Guid.Parse(request.ParentId);
            var result = await this.wikiRepository.UpdateAsync(entity);
            return FromResult(result);
        }
    }

    public class CreateWikiRequest 
    {
        public string Title { get; set; }
        public string? Markdown { get; set; }
        public string? ParentId { get; set; }
    }

    public class UpdateWikiRequest
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string? Markdown { get; set; }
        public string? ParentId { get; set; }        
    }
}
