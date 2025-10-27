using Catalog.Application.DTOs;
using MediatR;

namespace Catalog.Application.Features.Locations.Commands.UpdateLocation;

public class UpdateLocationCommand : IRequest<LocationDTO?>
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Type { get; set; }
}