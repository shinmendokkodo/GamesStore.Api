using GamesStore.Api.Extensions.Authorization;
using GamesStore.Api.Extensions.Cors;
using GamesStore.Api.Extensions.Data;
using GamesStore.Api.Extensions.ErrorHandling;
using GamesStore.Api.Extensions.Routes;
using GamesStore.Api.Extensions.Swagger;
using GamesStore.Api.Middleware;
using GamesStore.Api.OpenApi;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRepositories(builder.Configuration);

builder.Services.AddAuthentication().AddJwtBearer().AddJwtBearer("Auth0");
builder.Services.AddGameStoreAuthorization();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new(1.0);
    options.AssumeDefaultVersionWhenUnspecified = true;
})
.AddApiExplorer(options => options.GroupNameFormat = "'v'VVV");

builder.Services.AddGamesStoreCors(builder.Configuration);

builder.Services.AddSwaggerGen()
        .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
        .AddEndpointsApiExplorer();

/*
// Json Logging
builder.Logging.AddJsonConsole(options =>
{
    options.JsonWriterOptions = new()
    {
        Indented = true
    };
});
*/

var app = builder.Build();

app.UseExceptionHandler(handlerApp => handlerApp.ConfigureExceptionHandler());
app.UseMiddleware<RequestTimingMiddleware>();

await app.Services.InitializeDbAsync();

app.UseHttpLogging();

app.MapEndPoints();

app.UseCors();

app.UseSwaggerDocumentation();

app.Run();