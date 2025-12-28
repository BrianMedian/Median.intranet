
using Median.Core.Models.Common;
using Median.Intranet.Common;
using Median.Intranet.DAL.Contracts;
using Median.Intranet.Models;
using Median.Intranet.Models.Emails;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Median.Intranet.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly IDocumentEntityRepository _documentEntityRepository;
        private readonly IEmailTypeRepository _emailTypeRepository;
        private readonly IEmailLogRepository _emailLogRepository;
        private readonly SmtpSettings _settings;
        private readonly ILogger<EmailService> logger;
        private readonly IProfileDataRepository _profileDataRepository;
        public EmailService(IEmailTemplateRepository emailTemplateRepository, IOptions<SmtpSettings> options, IDocumentEntityRepository documentEntityRepository, IEmailTypeRepository emailTypeRepository, ILogger<EmailService> logger, IEmailLogRepository emailLogRepository, IProfileDataRepository profileDataRepository)
        {
            _emailTemplateRepository = emailTemplateRepository;
            _settings = options.Value;
            _documentEntityRepository = documentEntityRepository;
            _emailTypeRepository = emailTypeRepository;
            this.logger = logger;
            _emailLogRepository = emailLogRepository;
            _profileDataRepository = profileDataRepository;
        }

        public async Task<Result<bool>> SendFileToRecipient(string toEmail, Guid fileId, Guid sendingUserId)
        {
            try
            {
                //get the file
                var documentResult = await _documentEntityRepository.GetByIdAsync(fileId);
                if (documentResult.Failure)
                {
                    return false;
                }
                var documentAttachment = CreateAttachmentFromDocument(documentResult.Value);
                var placeholders = new Dictionary<string, string> { };
                //{
                //    { "FileId", fileId.ToString() },
                //    { "RecipientEmail", toEmail }
                //};

                var result = await SendEmail(sendingUserId, EmailTypes.SendFileEmail, toEmail, string.Empty, placeholders, new List<EmailAttachment> { documentAttachment });
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail<bool>(Errors.General.FromException(ex));
            }
        }

        public async Task<Result<bool>> SendBusinessCardToRecipient(string toEmail, Guid sendingUserId)
        {
            try
            {                
                var result = await SendEmail(sendingUserId, EmailTypes.SendFileEmail, toEmail, string.Empty, null, null);
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail<bool>(Errors.General.FromException(ex));
            }
        }

        private EmailAttachment CreateAttachmentFromDocument(DocumentEntity document)
        {
            var content = File.ReadAllBytes(document.FilePath);
            return new EmailAttachment
            {
                FileName = document.FileName,
                ContentType = document.ContentType,
                Content = content
            };
        }

        private async Task<Result<EmailTypeDto>> loadEmailType(string id)
        {
            try
            {
                var emailTypeResult = await this._emailTypeRepository.GetByEmailType(id);
                if (emailTypeResult.Failure)
                {
                    return Result.Fail<EmailTypeDto>(emailTypeResult.Error);
                }
                return Result.Ok<EmailTypeDto>(emailTypeResult.Value);
            }
            catch (Exception ex)
            {
                this.logger.LogCritical(ex, ex.Message);
                return Result.Fail<EmailTypeDto>(Errors.General.FromException(ex));
            }
        }

        /// <summary>
        /// For debug, log the email content instead of sending
        /// </summary>
        /// <param name="mail"></param>
        private void DumpEmailToLog(MailMessage mail)
        {
            this.logger.LogInformation("---- Email Dump: ----");
            this.logger.LogInformation($"From: {mail.From.Address}");
            foreach (var to in mail.To)
            {
                this.logger.LogInformation($"To: {to.Address}");
            }
            this.logger.LogInformation($"Subject: {mail.Subject}");
            this.logger.LogInformation($"Body: {mail.Body}");
            foreach (var attachment in mail.Attachments)
            {
                this.logger.LogInformation($"Attachment: {attachment.Name}, ContentType: {attachment.ContentType.MediaType}");
            }
        }

        /// <summary>
        /// Will send the email based on typeid and placeholders
        /// </summary>
        /// <param name="typeid"></param>
        /// <param name="toEmail"></param>
        /// <param name="placeholders"></param>
        /// <param name="attachments"></param>
        /// <returns></returns>
        private async Task<Result<bool>> SendEmail(Guid sendingUserId, string typeid, string toEmail="", string fromEmail = "", Dictionary<string, string> placeholders = null, List<EmailAttachment> attachments = null)
        {           
            //prepare the log
            EmailLog log = new EmailLog();
            log.Status = "OK";
            log.FromEmail = "";
            log.ToEmail = "";
            log.EmailTypeId = typeid;
            log.TemplateId = Guid.Empty;
            log.UserId = sendingUserId;

            try
            {
                //get the email type
                var emailType = await loadEmailType(typeid);
                if (emailType.Failure)
                {
                    return false;
                }
                //get the template
                var templateResult = await _emailTemplateRepository.GetByIdAsync(emailType.Value.TemplateId);
                if (templateResult.Failure || templateResult.Value == null)
                {
                    this.logger.LogError("Failed to load email template with id {TemplateId}", emailType.Value.TemplateId);
                    return false;
                }
                //set default or provided to/from
                if (string.IsNullOrEmpty(fromEmail))
                {
                    fromEmail = _settings.From;
                }
                if(string.IsNullOrEmpty(toEmail))
                {
                    toEmail = _settings.To;
                }
                //set the log data for to/from
                log.ToEmail = toEmail;
                log.FromEmail = fromEmail;

                //get the actual template
                var template = templateResult.Value;
                //set the template id for log
                log.TemplateId = template.Id;

                //replace placeholders
                string replacedContent = ReplacePlaceholders(template.Content, placeholders);
                replacedContent = await ReplaceProfileData(replacedContent, sendingUserId);
                //create the mail message
                using var mail = new MailMessage();
                mail.From = new MailAddress(fromEmail);
                mail.To.Add(toEmail);
                mail.Subject = template.Subject;
                mail.Body = replacedContent;
                mail.IsBodyHtml = true;

                if (attachments != null)
                {
                    foreach (var attachment in attachments)
                    {
                        if (attachment.Content == null || attachment.Content.Length == 0)
                        {
                            continue; //skip empty attachments
                        }
                        var stream = new MemoryStream(attachment.Content);
                        mail.Attachments.Add(
                            new Attachment(stream, attachment.FileName, attachment.ContentType)
                        );
                    }
                }

                using var client = new SmtpClient(_settings.Host, _settings.Port)
                {
                    Credentials = new NetworkCredential(
                        _settings.Username,
                        _settings.Password),
                    EnableSsl = _settings.EnableSsl
                };

                //if email is disabled, just log the email instead of sending
                if (!_settings.EmailEnabled)
                {
                    DumpEmailToLog(mail);
                }
                else
                {
                    //send email
                    await client.SendMailAsync(mail);
                }                               
                return Result.Ok<bool>(true);
            }
            catch (Exception ex)
            {                
                this.logger.LogCritical(ex, ex.Message);
                //set the log status
                log.Status = ex.Message;
                return Result.Fail<bool>(Errors.General.FromException(ex));
            }
            finally
            {
                var result = await this._emailLogRepository.CreateLogAsync(log);
                if (result.Failure)
                {
                    this.logger.LogError("Error saving email log");
                    this.logger.LogError(result.Error.Message);
                }
            }
        }

        private string ReplacePlaceholders(string content, Dictionary<string, string> placeholders)
        {
            foreach (var placeholder in placeholders)
            {
                content = content.Replace($"{{{{{placeholder.Key}}}}}", placeholder.Value);
            }
            return content;
        }

        private async Task<string> ReplaceProfileData(string content, Guid sendingUserId)
        {
            //get the profile data
            var profileDataResult = await this._profileDataRepository.GetByUserIdAsync(sendingUserId);
            if (profileDataResult.Failure)
            {
                this.logger.LogError(profileDataResult.Error.Message);
                return content;
            }
            string result = content;
            result = result.Replace("{{median.navn}}", profileDataResult.Value.FullName);
            result = result.Replace("{{median.titel}}", profileDataResult.Value.Position);
            result = result.Replace("{{median.email}}", profileDataResult.Value.Email);
            result = result.Replace("{{median.imageurl}}", profileDataResult.Value.ProfilePictureUrl);
            return result;
        }       
    }
}
