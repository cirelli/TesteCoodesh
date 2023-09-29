using Data;
using Microsoft.EntityFrameworkCore;
using PokeApiNet;
using PokeApiWrapper;
using Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options => options.UseSqlite("name=ConnectionStrings:DefaultConnection"));

builder.Services.AddScoped<PokeApiClient>();
builder.Services.AddScoped<PokeWrapper>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<RepositoryWrapper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
