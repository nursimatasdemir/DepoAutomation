using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Catalog.Infrastructure;
using Catalog.Application.Abstractions;
using Catalog.API.Controllers;
using Catalog.API.Services;
using Catalog.Application.Features.Products.Commands.CreateProduct;
using Catalog.Application.Features.Products.Commands.CreateProduct.Validation;
using FluentValidation;
using Catalog.API.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;


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


builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Catalog.Application.Abstractions.IApplicationDbContext).Assembly));

builder.Services.AddScoped<IApplicationDbContext, CatalogDbContext>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CatalogDbContext>(options => options.UseNpgsql(connectionString));

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

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
