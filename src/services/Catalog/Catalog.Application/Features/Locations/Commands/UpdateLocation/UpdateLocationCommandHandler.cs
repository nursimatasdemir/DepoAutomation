using Catalog.Application.DTOs;
using Catalog.Application.Abstractions;
using Catalog.Domain;
using FluentValidation;
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
        var locationToUpdate = await _context.Locations.FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);

        if (locationToUpdate == null)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure("Location",
                    $"Verilen {request.Id} numaralı lokasyon kayıtına ulaşılamadı. Id numarasını veya lokasyon bilgisinin varlığını kontrol edin.")
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