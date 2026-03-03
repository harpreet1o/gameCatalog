using GamecatalogAPI.Data;
using GamecatalogAPI.Models.Domain;
using GamecatalogAPI.Models.DTO;
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

        public async Task<List<Game>> GetAllAsync(GameQueryParameters queryParameters)
        {
            // 1. Validation Logic
            var pageNumber = queryParameters.PageNumber < 1 ? 1 : queryParameters.PageNumber;
            var pageSize = (queryParameters.PageSize < 1 || queryParameters.PageSize > 20)
                           ? 6 : queryParameters.PageSize;

            var games = dbContext.Game.AsQueryable();

            // 2. Filtering
            if (!string.IsNullOrEmpty(queryParameters.Search))
            {
                games = games.Where(x => x.Name.Contains(queryParameters.Search) ||
                                         x.Genre.Contains(queryParameters.Search));
            }

            // 3. Dynamic Sorting
            if (!string.IsNullOrEmpty(queryParameters.SortBy))
            {
                if (queryParameters.SortBy.Equals("Price", StringComparison.OrdinalIgnoreCase))
                {
                    games = queryParameters.IsDescending
                        ? games.OrderByDescending(x => x.Price)
                        : games.OrderBy(x => x.Price);
                }
                else // Default to Name sorting
                {
                    games = queryParameters.IsDescending
                        ? games.OrderByDescending(x => x.Name)
                        : games.OrderBy(x => x.Name);
                }
            }
            else
            {
                games = games.OrderBy(x => x.Name);
            }

            // 4. Paging Math
            var skip = (pageNumber - 1) * pageSize;

            return await games
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
