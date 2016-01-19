using FileHelpers;

namespace Demo.Models
{
    [DelimitedRecord(",")]
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}