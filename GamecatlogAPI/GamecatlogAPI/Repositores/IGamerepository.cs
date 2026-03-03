using GamecatalogAPI.Models.Domain;
namespace GamecatalogAPI.Repositores
{
    public interface IGamerepository
    {
        Task<List<Game>> GetAllAsync(string? serach = null);

        Task<Game?>GetByIdAsync(Guid id);

        Task<Game?> CreateAsync(Game game);
        
        Task<Game?> UpdateAsync(Guid id, Game game);

        Task<Game?> DeleteAsync(Guid id);


    }
}
