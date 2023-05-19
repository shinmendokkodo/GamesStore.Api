namespace GamesStore.Api.Extensions.Cors;

public static class CorsExtensions
{
    private const string allowedOriginsSetting = "AllowedOrigins";

    public static IServiceCollection AddGamesStoreCors(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(corsBuilder =>
            {
                var allowedOrigins = configuration["AllowedOrigins"]
                    ?? throw new InvalidOperationException("AllowedOrigins not found");

                corsBuilder.WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithExposedHeaders("X-Pagination");
            });
        });

        return services;
    }
}