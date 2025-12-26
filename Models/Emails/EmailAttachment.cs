namespace Median.Intranet.Models.Emails
{
    public class EmailAttachment
    {
        public string FileName { get; set; } = default!;
        public byte[] Content { get; set; } = default!;
        public string ContentType { get; set; } = "application/octet-stream";
    }

}
