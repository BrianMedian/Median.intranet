using Median.Intranet.DAL.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Median.Intranet.Controllers
{
    [Route("api/emailtypes")]
    [ApiController]
    public class EmailTypeController : BaseController
    {
        private readonly IEmailTypeRepository emailTypeRepository;
        private readonly ILogger<EmailTypeController> logger;
        public EmailTypeController(IEmailTypeRepository emailTypeRepository, ILogger<EmailTypeController> logger)
        {
            this.emailTypeRepository = emailTypeRepository;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmailTypes()
        {
            var types = await emailTypeRepository.GetAllAsync();
            return Ok(types);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmailType([FromBody] List<UpdateEmailTypeRequest> request)
        {
            try
            {
                foreach (var emailTypeRequest in request)
                {
                    await this.emailTypeRepository.Update(emailTypeRequest.EmailType, Guid.Parse(emailTypeRequest.TemplateId));
                }
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error updating email type with id {ex.Message}");                
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while updating the email type: {ex.Message}");
            }
        }
    }

    public class UpdateEmailTypeRequest
    {
        public string EmailType { get; set; }
        public string TemplateId { get; set; }
    }
}
