namespace Median.Intranet.Common
{
    public class SmtpSettings
    {
        public string Host { get; set; } = default!;
        public int Port { get; set; }
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string From { get; set; } = default!;
        public string To { get; set; } = default!;
        public bool EnableSsl { get; set; }
        public bool EmailEnabled { get; set; }
    }

}
