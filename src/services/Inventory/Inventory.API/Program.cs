using System.Text;
using Catalog.API.Services;
using Inventory.API.Middleware;
using Microsoft.EntityFrameworkCore;
using Inventory.Infrastructure;
using Inventory.Application.Abstraction;
using Inventory.Application.Features.StockTransactions.Commands.ReceiveStock;
using StackExchange.Redis;
using Inventory.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],

            ValidateAudience = true,
            ValidAudience = builder.Configuration["JwtSettings:Audience"],

            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"])),

            ValidateLifetime = true
        };
    });
builder.Services.AddAuthorization();

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
builder.Services.AddControllers().AddFluentValidationValidators();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Lütfen 'Bearer' ve ardından IdentityService'ten aldığınız JWT Token'ı girin.  \n\n",
        
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string [] {}
        }
    });
});

builder.Services.AddHostedService<StockCacheWarmer>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

