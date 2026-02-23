using GamecatalogAPI.Data;
using GamecatalogAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace GamecatalogAPI.Repositores
{
    public class SQLGameRepository : IGamerepository
    {
        private readonly GamesDBContext dbContext;
        public SQLGameRepository(GamesDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<Game>> GetAllAsync()
        {
          return await dbContext.Game.ToListAsync();
        }
    }
}
