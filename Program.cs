using GamesStore.Api.Extensions.Authorization;
using GamesStore.Api.Extensions.Data;
using GamesStore.Api.Extensions.Routes;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRepositories(builder.Configuration);

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddGameStoreAuthorization();

var app = builder.Build();

await app.Services.InitializeDbAsync();

app.MapEndPoints();

app.Run();
