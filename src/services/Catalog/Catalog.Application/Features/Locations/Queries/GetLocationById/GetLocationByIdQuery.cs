using Catalog.Application.DTOs;
using MediatR;

namespace Catalog.Application.Features.Locations.Queries.GetLocationById;

public class GetLocationByIdQuery : IRequest<LocationDTO?>
{
    public Guid Id { get; set; }
}