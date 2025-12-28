namespace Median.Intranet.Models.Emails
{
    public class EmailLog : BaseEntity
    {
        public string EmailTypeId { get; set; } = string.Empty; 
        public string FromEmail { get; set; } = string.Empty;
        public string ToEmail { get; set; } = string.Empty;
        public Guid TemplateId { get; set; } = Guid.Empty;
        public string Status { get; set; } = string.Empty;
        public Guid UserId { get; set; } = Guid.Empty;
    }
}
