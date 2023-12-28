using Demo.Data;
using Demo.Data.Entities;
using Demo.Data.Repository;
using Demo.Service.Core;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// DI . Singleton, Scope, Transient
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlServer("Server=localhost;Database=demo;user id=sa; password=yourStrong(!)Password"));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//var context = new AppDbContext();

//string id1 = "e31bdd0f-ac79-45c1-baa3-df2ba533146c";
//string id2 = "138d9376-fc54-4d94-86cc-3ea7d22ca2dc";
//string id3 = "382072aa-ecde-4752-85b3-aef54ec61660";

//Guid Guid1 = Guid.Parse(id1);
//Guid Guid2 = Guid.Parse(id2);
//Guid Guid3 = Guid.Parse(id3);

//context.InitializeData(new List<Product>
//{
//    new Product(Guid1, "P01", "com", 5, 30),
//    new Product(Guid2, "P02", "ao", 6, 50),
//    new Product(Guid3, "C03", "gao", 2, 60)
//});

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

