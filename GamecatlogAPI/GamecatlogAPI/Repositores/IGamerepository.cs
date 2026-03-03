using GamecatalogAPI.Models.Domain;
using GamecatalogAPI.Models.DTO;
namespace GamecatalogAPI.Repositores
{
    public interface IGamerepository
    {
        Task<List<Game>> GetAllAsync(GameQueryParameters queryParameters);

        Task<Game?>GetByIdAsync(Guid id);

        Task<Game?> CreateAsync(Game game);
        
        Task<Game?> UpdateAsync(Guid id, Game game);

        Task<Game?> DeleteAsync(Guid id);


    }
}
