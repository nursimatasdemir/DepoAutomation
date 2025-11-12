using Inventory.Application.DTOs;
using MediatR;
using StackExchange.Redis;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;

namespace Inventory.Application.Features.StockTransactions.Queries.GetStockLevel;

public class GetStockLevelQueryHandler : IRequestHandler<GetStockLevelQuery, StockLevelDTO>
{
    private readonly IConnectionMultiplexer _redis;

    public GetStockLevelQueryHandler(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task<StockLevelDTO> Handle(GetStockLevelQuery request, CancellationToken cancellationToken)
    {
        var redisDb = _redis.GetDatabase();
        var productId = request.ProductId;

        var productExists = await redisDb.SetContainsAsync("valid_products", request.ProductId.ToString());
        

        if (!productExists)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure("productId", $"ProductId (...) geçerli bir ürün Id numarası değil.")
            });
        }
        decimal totalQuantity = 0;
            
        var server = _redis.GetServer("localhost", 6379);

        var pattern = $"stock:{request.ProductId}:*";
        
        var keys = server.Keys(pattern: pattern);

        foreach (var key in keys)
        {
            var valueString = await redisDb.StringGetAsync(key);
            
            if (valueString.TryParse(out double quantity))
            {
                totalQuantity += (decimal)quantity;
            }
        }

        return new StockLevelDTO
        {
            ProductId= request.ProductId,
            TotalQuantity = totalQuantity,
        };
    }
}