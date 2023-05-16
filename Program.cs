using GamesStore.Api.Extensions.Authorization;
using GamesStore.Api.Extensions.Cors;
using GamesStore.Api.Extensions.Data;
using GamesStore.Api.Extensions.ErrorHandling;
using GamesStore.Api.Extensions.Routes;
using GamesStore.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRepositories(builder.Configuration);

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddGameStoreAuthorization();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new(1.0);
    options.AssumeDefaultVersionWhenUnspecified = true;
});

builder.Services.AddGamesStoreCors(builder.Configuration);

// Json Logging
// builder.Logging.AddJsonConsole(options =>
// {
//     options.JsonWriterOptions = new()
//     {
//         Indented = true
//     };
// });

var app = builder.Build();

app.UseExceptionHandler(handlerApp => handlerApp.ConfigureExceptionHandler());
app.UseMiddleware<RequestTimingMiddleware>();

await app.Services.InitializeDbAsync();

app.UseHttpLogging();

app.MapEndPoints();

app.UseCors();

app.Run();
