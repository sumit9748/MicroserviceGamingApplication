

using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using Play.Common;
using Play.Common.MongoDB;
using MassTransit;
using Play.Common.MassTransit;
using System.Text.Json;
using Play.Catalog.Entities;
using Play.Catalog.Middleware;
using Play.Common.Middleware;


var builder = WebApplication.CreateBuilder(args);

BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

// Add services to the container.


builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = new MongoClient(builder.Configuration.GetConnectionString("key"));
    // pick database name from config
    return client.GetDatabase(builder.Configuration["ServiceSetings:ServiceName"]);
});
builder.Services.AddMassTransitWithRabbitMQ("inventory");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IMongoRepositry<Item>>(ServiceProvider =>
{
    var database = ServiceProvider.GetService<IMongoDatabase>();
    return new MongoRepositry<Item>(database, "items");
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
