using Catalog.Application.Abstractions;
using Catalog.Application.DTOs;
using Catalog.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Features.Locations.Queries.GetLocationById;

public class GetLocationByIdQueryHandler : IRequestHandler<GetLocationByIdQuery, LocationDTO?>
{
    private readonly IApplicationDbContext _context;

    public GetLocationByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<LocationDTO?> Handle(GetLocationByIdQuery request, CancellationToken cancellationToken)
    {
        var location = await _context.Locations
            .FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);

        if (location == null)
        {
            return null;
        }

        return new LocationDTO
        {
            Id = location.Id,
            Type = location.Type,
            Code = location.Code,
        };
    }
}