namespace GamecatalogAPI.Models.DTO
{
    public record GameQueryParameters(
    string? Search = null,
    int PageNumber = 1,
    int PageSize = 6,
    string? SortBy = "Name",
    bool IsDescending = false
);
}
