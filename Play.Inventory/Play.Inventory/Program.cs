
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using Play.Common;
using Play.Common.MongoDB;
using Play.Inventory.service.Middleware;
using Polly.Timeout;
using Polly;
using Play.Common.MassTransit;
using System.Text.Json;
using Play.Inventory.Entities;

var builder = WebApplication.CreateBuilder(args);

BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
// Add services to the container.


builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(builder.Configuration.GetConnectionString("key")));

builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(builder.Configuration["ServiceSetings:ServiceName"]);
});

builder.Services.AddMassTransitWithRabbitMQ("inventory");
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMongoRepositry<InventoryItem>>(sp =>
{
    var database = sp.GetRequiredService<IMongoDatabase>();
    return new MongoRepositry<InventoryItem>(database, "inventoryitems");
});

builder.Services.AddScoped<IMongoRepositry<CatalogItem>>(sp =>
{
    var database = sp.GetRequiredService<IMongoDatabase>();
    return new MongoRepositry<CatalogItem>(database, "catalogitems");
});

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
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
