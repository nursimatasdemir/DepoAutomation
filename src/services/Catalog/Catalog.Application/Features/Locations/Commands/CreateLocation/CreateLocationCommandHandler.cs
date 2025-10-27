using Catalog.Application.Abstractions;
using Catalog.Domain;
using MediatR;

namespace Catalog.Application.Features.Locations.Commands.CreateLocation;

public class CreateLocationCommandHandler : IRequestHandler<CreateLocationCommand, Guid>
{
    public readonly IApplicationDbContext _context;

    public CreateLocationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
    {
        var newLocation = new Location
        {
            Id = Guid.NewGuid(),
            Code = request.Code,
            Type = request.Type
        };
        
        await _context.Locations.AddAsync(newLocation, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return newLocation.Id;
    }
}