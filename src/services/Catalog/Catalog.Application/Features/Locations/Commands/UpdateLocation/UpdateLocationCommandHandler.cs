using Catalog.Application.DTOs;
using Catalog.Application.Abstractions;
using Catalog.Domain;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace Catalog.Application.Features.Locations.Commands.UpdateLocation;

public class UpdateLocationCommandHandler : IRequestHandler<UpdateLocationCommand, LocationDTO?>
{
    private readonly IApplicationDbContext _context;

    public UpdateLocationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<LocationDTO?> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
    {
        var locationExists = await _context.Locations.AnyAsync(c => c.Id == request.Id, cancellationToken);
        if (!locationExists)
        {
            throw new FluentValidation.ValidationException(new []
            {
                new ValidationFailure("Location", $"Verilen {request.Id} numaralı lokasyon kayıtına ulaşılamadı.")
            });
        }
        var locationToUpdate = await _context.Locations
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (locationToUpdate == null)
        {
            throw new FluentValidation.ValidationException(new []
            {
                new ValidationFailure("LocationToUpdate", $"Verilen {request.Id} numaralı lokasyon kayıtı boş olamaz.")
            });
        }
        
        locationToUpdate.Code = request.Code;
        locationToUpdate.Type = request.Type;
        await _context.SaveChangesAsync(cancellationToken);

        return new LocationDTO
        {
            Id = locationToUpdate.Id,
            Code = locationToUpdate.Code,
            Type = locationToUpdate.Type,
        };

    }
}