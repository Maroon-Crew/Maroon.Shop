using Maroon.Shop.Api.Data.Repositories;
using Maroon.Shop.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
//    .AddJsonOptions(o =>
//{
//    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
//});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.MapType<decimal>(() => new OpenApiSchema { Type = "number", Format = "decimal" });
    c.SupportNonNullableReferenceTypes();
});
builder.Services.AddDbContext<ShopContext>(options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));
builder.Services.AddTransient<AddressRepository>();
builder.Services.AddTransient<ProductRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
