namespace Median.Intranet.Models
{
    public class WikiEntity : BaseEntity
    {
        public string Title { get; set; }
        public string Markdown { get; set; }
        public Guid ParentId { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
