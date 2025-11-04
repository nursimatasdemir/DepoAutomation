using Inventory.Application.Abstraction;
using Inventory.Infrastructure;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Inventory.API.Services;

public class StockCacheWarmer : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<StockCacheWarmer> _logger;

    public StockCacheWarmer(IServiceProvider serviceProvider, ILogger<StockCacheWarmer> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stock cache warmer starting.");

        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<IInventoryDbContext>();
            var redis = scope.ServiceProvider.GetRequiredService<IConnectionMultiplexer>();
            var redisDb = redis.GetDatabase();

            try
            {
                _logger.LogInformation("Warming up Redis cache from PostgreSQL");
                
                var stockLevels = await dbContext.StockTransactions
                    .AsNoTracking()
                    .GroupBy(st => new {st.ProductId, st.LocationId})
                    .Select(g => new
                    {
                        g.Key.ProductId,
                        g.Key.LocationId,
                        totalQuantity = g.Sum(st => st.QuantityChange)
                    })
                    .ToListAsync(cancellationToken);
                _logger.LogInformation($"Found {stockLevels.Count} stock records in PostgreSQL.");

                foreach (var stock in stockLevels)
                {
                    string redisKey = $"stock:{stock.ProductId}:{stock.LocationId}";
                    await redisDb.StringSetAsync(redisKey, (double)stock.totalQuantity);
                }
                
                _logger.LogInformation("Successfully warmed up Redis cache from PostgreSQL.");
                
                var validProductIds = await dbContext.ProductViews
                    .AsNoTracking()
                    .Select(p => p.Id.ToString())
                    .ToArrayAsync(cancellationToken);
                
                _logger.LogInformation($"Found {validProductIds.Length} product views from PostgreSQL.");

                if (validProductIds.Length > 0)
                {
                    string validProductsKey = "valid_products";
                    await redisDb.KeyDeleteAsync(validProductsKey);

                    var redisValues = validProductIds.Select(id => (RedisValue)id).ToArray();
                    await redisDb.SetAddAsync(validProductsKey, redisValues);
                    
                    _logger.LogInformation("Valid Product Ids warming completed.");
                }
                _logger.LogInformation("Successfully warmed up Redis cache from Redis");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured during Stock Cache warming.");
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping Stock Cache Warmer Service");
        return Task.CompletedTask;
    }
    
}