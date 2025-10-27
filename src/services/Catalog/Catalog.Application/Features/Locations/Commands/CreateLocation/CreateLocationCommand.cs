using Catalog.Domain;
using MediatR;

namespace Catalog.Application.Features.Locations.Commands.CreateLocation;

public class CreateLocationCommand : IRequest<Guid>
{
    public string Code { get; set; }
    public string Type { get; set; }
}