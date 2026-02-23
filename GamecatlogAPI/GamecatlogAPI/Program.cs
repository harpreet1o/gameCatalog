using Scalar.AspNetCore;
using GamecatalogAPI.Data;
using Microsoft.EntityFrameworkCore;
using GamecatalogAPI.Repositores;
using GamecatalogAPI.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<GamesDBContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("GameCatalogConnection")));

builder.Services.AddScoped<IGamerepository, SQLGameRepository>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<AutoMapperProfile>();
}, AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
