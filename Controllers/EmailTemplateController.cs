using Median.Intranet.DAL.Contracts;
using Median.Intranet.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Median.Intranet.Controllers
{
    [Route("api/emailtemplates")]
    [ApiController]
    public class EmailTemplateController : BaseController
    {
        private readonly IEmailTemplateRepository emailTemplateRepository;
        public EmailTemplateController(IEmailTemplateRepository emailTemplateRepository)
        {
            this.emailTemplateRepository = emailTemplateRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmailTemplates()
        {
            var templates = await emailTemplateRepository.GetAllAsync();
            return FromResult<List<Median.Intranet.Models.Emails.EmailTemplate>>(templates);
        }

        [HttpGet("{id}")]        
        public async Task<IActionResult> GetEmailTemplateById(Guid id)
        {
            var template = await emailTemplateRepository.GetByIdAsync(id);
            return FromResult(template);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmailTemplate([FromBody] CreateEmailTemplateRequest request)
        {
            var emailTemplate = new Median.Intranet.Models.Emails.EmailTemplate
            {
                Name = request.name,
                Subject = request.subject,
                Description = request.description,
                Key = NanoIdGenerator.Generate(prefix:"email")
            };
            var createResult = await emailTemplateRepository.CreateAsync(emailTemplate);
            return FromResult(createResult);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmailTemplate(Guid id, [FromBody] UpdateEmailTemplateRequest request)
        {
            var existingTemplateResult = await emailTemplateRepository.GetByIdAsync(id);
            if (!existingTemplateResult.Success)
            {
                return FromResult(existingTemplateResult);
            }
            var existingTemplate = existingTemplateResult.Value;
            existingTemplate.Name = request.name;
            existingTemplate.Subject = request.subject;
            existingTemplate.Description = request.description;
            existingTemplate.Version += 1;
            existingTemplate.UpdatedAt = DateTime.UtcNow;
            existingTemplate.Content = request.content;
            var updateResult = await emailTemplateRepository.UpdateAsync(existingTemplate);
            return FromResult(updateResult);
        }

        [HttpDelete("{id}")]        
        public async Task<IActionResult> DeleteEmailTemplate(Guid id)
        {
            var deleteResult = await emailTemplateRepository.DeleteAsync(id);
            return FromResult(deleteResult);
        }
    }

    public class CreateEmailTemplateRequest
    {
        public string name { get; set; } = string.Empty;
        public string subject { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
    }

    public class UpdateEmailTemplateRequest
    {
        public Guid id { get; set; }
        public string name { get; set; } = string.Empty;
        public string subject { get; set; } = string.Empty;
        public string? description { get; set; } = string.Empty;
        public string? content { get; set; } = string.Empty;
    }
}
