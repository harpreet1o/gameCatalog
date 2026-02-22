namespace GamecatalogAPI.Models.DTO;

public record AddGameRequestDto(
    string Name,
    string Description,
    decimal Price,
    string Genre,
    string? GameImageURL
);