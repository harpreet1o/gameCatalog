using System.ComponentModel.DataAnnotations;

namespace GamecatalogAPI.Models.DTO;

public record AddGameRequestDto(
    [Required(ErrorMessage = "Name is required and cannot be empty")]
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
    string Name,

    [Required(ErrorMessage = "Description is required")]
    string Description,

    [Range(0.01, 10000.00, ErrorMessage = "Price must be between 0.01 and 10,000")]
    decimal Price,

    [Required(ErrorMessage = "Genre is required")]
    string Genre,
    [Url(ErrorMessage = "Invalid URL format")]
    string? GameImageURL
);