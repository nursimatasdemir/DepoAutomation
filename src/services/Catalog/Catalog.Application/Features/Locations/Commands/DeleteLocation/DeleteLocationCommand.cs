using Catalog.Application.DTOs;
using MediatR;

namespace Catalog.Application.Features.Locations.Commands.DeleteLocation;

public class DeleteLocationCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}