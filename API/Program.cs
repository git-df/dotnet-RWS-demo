using API.Hubs;
using API.Models;
using API.Services;
using Microsoft.Extensions.Configuration;
using System.Runtime;
var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RabbitMQOptions>(builder.Configuration.GetSection("RabbitMQOptions"));
builder.Services.AddHostedService<RabbitMQConsumerService>();
builder.Services.AddSignalR();
builder.Services.AddCors();

var app = builder.Build();

app.UseCors(policy => policy
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin());

app.MapHub<EventsHub>("events");
app.Run();
