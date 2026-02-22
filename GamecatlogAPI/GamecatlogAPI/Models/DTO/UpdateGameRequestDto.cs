namespace GamecatalogAPI.Models.DTO;

public record UpdateGameRequestDto(
   string Name,
   string Description,
   decimal Price,
   string Genre,
   string? GameImageURL
 );
