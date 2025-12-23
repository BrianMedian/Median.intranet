namespace Median.Intranet.Models
{
    public class DocumentEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Version { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public double FileSize { get; set; }
    }
}
