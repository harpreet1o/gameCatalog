using GamecatalogAPI.Data;
using GamecatalogAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

namespace GamecatalogAPI.Repositores
{
    public class SQLGameRepository : IGamerepository
    {
        private readonly GamesDBContext dbContext;
        public SQLGameRepository(GamesDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Game?> CreateAsync(Game game)
        {
          await dbContext.Game.AddAsync(game);
          await dbContext.SaveChangesAsync();
            return game;
        }

        public async Task<Game?> DeleteAsync(Guid id)
        {
            var existingGame = await dbContext.Game.FirstOrDefaultAsync(x => x.Id == id);
            if(existingGame == null)
            {
                return null;
            }
            dbContext.Game.Remove(existingGame);
            await dbContext.SaveChangesAsync();
            return existingGame;
        }

        public async Task<List<Game>> GetAllAsync(string? search = null, int pageNumber = 1, int pageSize = 6)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 6 : pageSize;
            var games = dbContext.Game.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                games = games.Where(x => x.Name.Contains(search) || x.Genre.Contains(search));
            }
            var skip = (pageNumber - 1) * pageSize;

            return await games
                .OrderBy(x=>x.Name)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
   
        }

        public async Task<Game?> GetByIdAsync(Guid id)
        {
          return await dbContext.Game.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Game?> UpdateAsync(Guid id, Game game)
        {
            var existingGame = await dbContext.Game.FirstOrDefaultAsync(x => x.Id == id);
            if (existingGame == null)
            {
                return null;
            }
            existingGame.Name = game.Name;
            existingGame.Description = game.Description;
            existingGame.Price = game.Price;
            existingGame.Genre = game.Genre;
            existingGame.GameImageURL = game.GameImageURL;

            await dbContext.SaveChangesAsync();
            return existingGame;
        }
    }
}
