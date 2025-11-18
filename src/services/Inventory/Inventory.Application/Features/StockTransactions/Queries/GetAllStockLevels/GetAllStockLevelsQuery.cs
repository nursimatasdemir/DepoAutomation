using Inventory.Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Inventory.Application.Features.StockTransactions.Queries.GetAllStockLevels;

public class GetAllStockLevelsQuery : IRequest<List<StockLevelDTO>>
{
    
}