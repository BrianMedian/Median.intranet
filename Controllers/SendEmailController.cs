using Median.Intranet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Median.Intranet.Controllers
{
    [Route("api/sendemail")]
    [ApiController]
    [AllowAnonymous]
    public class SendEmailController : BaseController
    {
        private readonly IEmailService _emailService;

        public SendEmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("file")]
        public async Task<IActionResult> SendFileEmail([FromBody] SendFileEmailRequest request)
        {
            var result = await this._emailService.SendFileToRecipient(request.To, Guid.Parse(request.FileId), Guid.Parse(request.UserId));
            return FromResult<bool>(result);
        }

        [HttpPost("businesscard")]
        public async Task<IActionResult> SendBusinessCardEmail([FromBody] SendBusinessCardEmailRequest request)
        {
            var result = await this._emailService.SendBusinessCardToRecipient(request.To, Guid.Parse(request.UserId));
            return FromResult<bool>(result);
        }
    }

    public class BaseSendEmailRequest
    {
        public string UserId { get; set; } = string.Empty; //this is the id of the user who sent it
        public string? To { get; set; } = default!;
        public string? From { get; set; } = default!;
        public string? EmailTypeId { get; set; }
    }

    public class SendFileEmailRequest : BaseSendEmailRequest
    {
        public string FileId { get; set; }
    }

    public class SendBusinessCardEmailRequest : BaseSendEmailRequest
    {
    }
}
