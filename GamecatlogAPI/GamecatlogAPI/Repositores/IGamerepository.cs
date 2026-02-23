using GamecatalogAPI.Models.Domain;
namespace GamecatalogAPI.Repositores
{
    public interface IGamerepository
    {
        Task<List<Game>> GetAllAsync();

        Task<Game?>GetByIdAsync(Guid id);

        Task<Game?> CreateAsync(Game game);


    }
}
