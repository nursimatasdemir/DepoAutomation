using Inventory.Application.DTOs;
using MediatR;
using StackExchange.Redis;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using Inventory.Application.Abstraction;

namespace Inventory.Application.Features.StockTransactions.Queries.GetAllStockLevels;

public class GetAllStockLevelsQueryHandler : IRequestHandler<GetAllStockLevelsQuery, List<StockLevelDTO>>
{
    private readonly IConnectionMultiplexer _redis;

    public GetAllStockLevelsQueryHandler(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task<List<StockLevelDTO>> Handle(GetAllStockLevelsQuery request, CancellationToken cancellationToken)
    {
        var redisDb = _redis.GetDatabase();
        var server = _redis.GetServer("localhost", 6379);

        var keys = server.Keys(pattern: "stock:*:*").ToArray();
        if (!keys.Any())
        {
            return new List<StockLevelDTO>();
        }
        
        var values = await redisDb.StringGetAsync(keys);
        
        var stockTotals = new Dictionary<Guid, decimal>();

        for (int i = 0; i < keys.Length; i++)
        {
            var key = keys[i].ToString();
            var parts = key.Split(':');

            if (parts.Length == 3 && Guid.TryParse(parts[1], out Guid productId))
            {
                if (values[i].TryParse(out double quantity))
                {
                    stockTotals.TryGetValue(productId, out var currentTotal);
                    stockTotals[productId] = currentTotal + (decimal)quantity;
                }
            }
        }
        
        var result = stockTotals
            .Select(pair=>new StockLevelDTO
            {
                ProductId = pair.Key,
                TotalQuantity = pair.Value,
            }).ToList();
        
        return result;
    }
    
}