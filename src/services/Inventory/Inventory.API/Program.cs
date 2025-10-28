using Microsoft.EntityFrameworkCore;
using Inventory.Infrastructure;
using Inventory.Application.Abstraction;
using Inventory.Application.Features.StockTransactions.Commands.ReceiveStock;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var redisConnectionString = builder.Configuration.GetValue<string>("Redis:ConnectionString");
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var redis = ConnectionMultiplexer.Connect(redisConnectionString);
    return redis;
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ReceiveStockCommand).Assembly));

builder.Services.AddScoped<IInventoryDbContext, InventoryDbContext>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<InventoryDbContext>(options => options.UseNpgsql(connectionString));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

