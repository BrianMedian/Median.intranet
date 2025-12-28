namespace Median.Intranet.Models.Dto
{
    public class UserDto
    {
        public Guid UserId { get; set; }        
        public string UserEmail { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;
    }
}
