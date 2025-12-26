
using Median.Intranet.Common;
using Median.Intranet.DAL.Contracts;
using Median.Intranet.Models;
using Median.Intranet.Models.Emails;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Median.Intranet.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly IDocumentEntityRepository _documentEntityRepository;
        private readonly IEmailTypeRepository _emailTypeRepository;
        private readonly SmtpSettings _settings;
        public EmailService(IEmailTemplateRepository emailTemplateRepository, IOptions<SmtpSettings> options, IDocumentEntityRepository documentEntityRepository, IEmailTypeRepository emailTypeRepository)
        {
            _emailTemplateRepository = emailTemplateRepository;
            _settings = options.Value;
            _documentEntityRepository = documentEntityRepository;
            _emailTypeRepository = emailTypeRepository;
        }

        public async Task<bool> SendFileToRecipient(string toEmail, Guid fileId)
        {
            //get the file
            var documentResult = await _documentEntityRepository.GetByIdAsync(fileId);
            if (documentResult.Failure)
            {
                return false;
            }
            var documentAttachment = CreateAttachmentFromDocument(documentResult.Value);
            var placeholders = new Dictionary<string, string>
            {
                { "FileId", fileId.ToString() },
                { "RecipientEmail", toEmail }
            };
            return await SendEmail("your-template-id-here", toEmail, placeholders, new List<EmailAttachment> { documentAttachment });
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

        public async Task<bool> SendEmailWithTemplate(string templateId, string toEmail, Dictionary<string, string> placeholders, List<EmailAttachment> attachments)
        {
            return await SendEmail(templateId, toEmail, placeholders, attachments);
        }
        private async Task<bool> SendEmail(string templateId, string toEmail, Dictionary<string, string> placeholders, List<EmailAttachment> attachments)
        {
            try
            {
                //get the template
                var templateResult = await _emailTemplateRepository.GetByIdAsync(Guid.Parse(templateId));
                if (templateResult.Failure)
                {
                    return false;
                }
                var template = templateResult.Value; //get the template
                string replacedContent = ReplacePlaceholders(template.Content, placeholders);
                using var mail = new MailMessage();
                mail.From = new MailAddress(_settings.From);
                mail.To.Add(toEmail);
                mail.Subject = template.Subject;
                mail.Body = replacedContent;
                mail.IsBodyHtml = true;

                foreach (var attachment in attachments)
                {
                    if(attachment.Content == null || attachment.Content.Length == 0)
                    {
                        continue; //skip empty attachments
                    }
                    var stream = new MemoryStream(attachment.Content);
                    mail.Attachments.Add(
                        new Attachment(stream, attachment.FileName, attachment.ContentType)
                    );
                }

                using var client = new SmtpClient(_settings.Host, _settings.Port)
                {
                    Credentials = new NetworkCredential(
                        _settings.Username,
                        _settings.Password),
                    EnableSsl = true
                };

                await client.SendMailAsync(mail);
                return true;
            }
            catch (Exception ex)
            {
                //log exception
                return false;
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
    }
}
