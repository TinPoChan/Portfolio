using System.ComponentModel.DataAnnotations;

namespace BlazorApp2.Data
{
    public class PortfolioItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        public string Description { get; set; }

        [Url]
        public string ProjectUrl { get; set; }

        [Url]
        public string ImageUrl { get; set; }
    }
}
