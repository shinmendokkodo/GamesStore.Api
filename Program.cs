using GamesStore.Api.Extensions.Routes;
using GamesStore.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IGamesRepository, InMemoryGamesRepository>();

var conn = builder.Configuration.GetConnectionString("GameStoreContext");

var app = builder.Build();

app.MapEndPoints();

app.Run();
