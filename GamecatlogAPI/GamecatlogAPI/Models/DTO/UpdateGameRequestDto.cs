using System.ComponentModel.DataAnnotations;

namespace GamecatalogAPI.Models.DTO;

public record UpdateGameRequestDto(

[Required(ErrorMessage = "Name is required")]
    string Name,

    [Required(ErrorMessage = "Description is required")]
    string Description,

    [Range(0.01, 1000000.00, ErrorMessage = "Price must be positive")]
    decimal Price,

    [Required(ErrorMessage = "Genre is required")]
    string Genre,

    [Url(ErrorMessage = "Invalid URL format")]
    string? GameImageURL
 );
