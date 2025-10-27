using Catalog.Application.Abstractions;
using Catalog.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Features.Locations.Queries.GetLocations;

public class GetLocationsQueryHandler : IRequestHandler<GetLocationsQuery, List<LocationDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetLocationsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<LocationDTO>> Handle(GetLocationsQuery request, CancellationToken cancellationToken)
    {
        var locations = await _context.Locations
            .Select(c => new LocationDTO
            {
                Id = c.Id,
                Code = c.Code,
                Type = c.Type,
            })
            .ToListAsync(cancellationToken);
        return locations;
    }
}