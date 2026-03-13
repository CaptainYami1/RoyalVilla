using Microsoft.EntityFrameworkCore;
using RoyalVilla_API.Context;
using RoyalVilla_API.Models;
using RoyalVilla_API.Models.DTOs;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<RoyalVilleDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(o =>
{
    o.CreateMap<VillaCreateDTO, Villa>();
    o.CreateMap<VillaUpdateDTO, Villa>();
});

var app = builder.Build();
//await SeedDataAsync(app);

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

//static async Task SeedDataAsync(WebApplication app)
//{
//    using var scope = app.Services.CreateAsyncScope();
//    var context = scope.ServiceProvider.GetRequiredService<DbContext>();

//    await context.Database.MigrateAsync();
//}
