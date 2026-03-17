using Catalog.Application.Abstractions;
using Catalog.Domain;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Catalog.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    public readonly IApplicationDbContext _context;
    public readonly IConnectionMultiplexer _redis;
    private readonly ILogger<CreateProductCommand> _logger;

    public CreateProductCommandHandler(IApplicationDbContext context, IConnectionMultiplexer redis, ILogger<CreateProductCommand> logger)
    {
        _context = context;
        _redis = redis;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        
        var newProduct = new Product
        {
            Id = Guid.NewGuid(),
            Sku = request.Sku,
            Name = request.Name,
            Barcode = request.Barcode,
            CategoryId = request.CategoryId,
        };
        
        await _context.Products.AddAsync(newProduct, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        try
        {
            var redisDb = _redis.GetDatabase();
            await redisDb.SetAddAsync("valid_products", newProduct.Id.ToString());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ürün oluşturuldu ANCAK Redis cache güncellenemedi.");
        }
        
        
        return newProduct.Id;
    }
}