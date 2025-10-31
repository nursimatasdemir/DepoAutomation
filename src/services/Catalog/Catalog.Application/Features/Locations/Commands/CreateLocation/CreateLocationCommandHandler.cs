using Catalog.Application.Abstractions;
using Catalog.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        var locationCodeExists = await _context.Locations.AnyAsync(x => x.Code == request.Code, cancellationToken);
        if (locationCodeExists)
        {
            throw new FluentValidation.ValidationException("Belirtilen lokasyon kodu mevcut.");
        }
        var locationTypeExists = await _context.Locations.AnyAsync(c => c.Type == request.Type, cancellationToken);
        if (locationTypeExists)
        {
            throw new FluentValidation.ValidationException("Belirtilen lokasyon mevcut.");
        }
        
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