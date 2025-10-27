using Catalog.Application.DTOs;
using Catalog.Domain;
using MediatR;

namespace Catalog.Application.Features.Locations.Queries.GetLocations;

public class GetLocationsQuery : IRequest<List<LocationDTO>>
{
    
}