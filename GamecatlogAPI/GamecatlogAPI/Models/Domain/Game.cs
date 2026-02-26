using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamecatalogAPI.Models.Domain
{
    public class Game
    {
        // required ask them to submit something can't be empty
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } = 0.00m;
        [Required]
        [MaxLength(100)]
        public string Genre { get; set; } = string.Empty;
        //used the ? to make it nullable if the user doessn't want to add image
        [MaxLength(5000)]
        public string? GameImageURL { get; set; }
    }
}
