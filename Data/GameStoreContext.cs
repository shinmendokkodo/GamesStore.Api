using GamesStore.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GamesStore.Api.Data;

public class GameStoreContext : DbContext
{
    public GameStoreContext(DbContextOptions<GameStoreContext> options)
        : base(options)
    {

    }

    public DbSet<Game> Games => Set<Game>();
}