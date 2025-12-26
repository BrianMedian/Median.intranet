namespace Median.Intranet.Models.Emails
{
    public class EmailTemplate : BaseEntity
    {
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public int Version { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// Stable logical identifier used for programmatic lookup (e.g. "email.invoice.created")
        /// </summary>
        public string Key { get; set; }
    }
}
