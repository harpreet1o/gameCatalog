
using GamecatalogAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace GamecatalogAPI.Data
{
    public class GamesDBContext : DbContext
    {
        public GamesDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }
        public DbSet<Game> Game { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Game>()
                .Property(x=> x.Price)
                .HasColumnType("decimal(18,2)");

        }
    }
}
