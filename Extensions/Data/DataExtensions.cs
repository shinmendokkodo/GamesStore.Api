using GamesStore.Api.Data;
using GamesStore.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GamesStore.Api.Extensions.Data;

public static class DataExtensions
{
    public static async Task InitializeDbAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        await db.Database.MigrateAsync();

        var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DataBase Initializer");
        logger.LogInformation(5, "Database is ready.");
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        var connString = configuration.GetConnectionString("GameStoreContext");
        services.AddSqlServer<GameStoreContext>(connString)
            .AddScoped<IGamesRepository, EntityFrameworkGamesRepository>();

        return services;
    }
}