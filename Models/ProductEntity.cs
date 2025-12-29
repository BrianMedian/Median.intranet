namespace Median.Intranet.Models
{
    public class ProductEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Price { get; set; } //the from price in øre
        public string Tags { get; set; }                                       
    }
}
