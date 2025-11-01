using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Play.Common;
using Play.Common.MassTransit;
using Play.Common.Middleware;
using Play.Common.MongoDB;
using Play.OrderService.DbContextClass;
using Play.OrderService.Entities;
using Play.OrderService.Repo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(builder.Configuration.GetConnectionString("key2")));

builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(builder.Configuration["ServiceSetings:ServiceName"]);
});
builder.Services.AddDbContext<OrderDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("key1"));
});
builder.Services.AddMassTransitWithRabbitMQ("inventory");
builder.Services.AddScoped<IOrderClass,OrderClass>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMongoRepositry<UserBucket>>(sp =>
{
    var database = sp.GetRequiredService<IMongoDatabase>();
    return new MongoRepositry<UserBucket>(database, "userbucket");
});


var app = builder.Build();

app.UseGlobalExceptionHandler();
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
