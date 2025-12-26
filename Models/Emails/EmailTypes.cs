namespace Median.Intranet.Models.Emails
{
    public class EmailTypes
    {
        public const string SendFileEmail = "email.send.file";
        public const string SendBusinessCardEmail = "email.send.business.card";

        public static List<string> All => new List<string>
        {
            SendFileEmail,
            SendBusinessCardEmail
        };
    }
}
