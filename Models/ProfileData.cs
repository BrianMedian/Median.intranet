namespace Median.Intranet.Models
{
    public class ProfileData : BaseEntity
    {
        public Guid UserId { get; set; }        
        public string FullName { get; set; }
        public string Position { get; set; }                
        public string PhoneNumber { get; set; }        
        public string Email { get; set; }
        public string OnlineCardUrl { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}
