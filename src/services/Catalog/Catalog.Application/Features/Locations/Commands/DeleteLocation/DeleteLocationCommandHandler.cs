using Catalog.Application.Abstractions;
using Catalog.Application.DTOs;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Features.Locations.Commands.DeleteLocation;

public class DeleteLocationCommandHandler : IRequestHandler<DeleteLocationCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteLocationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
    {
        var locationToDelete = await _context.Locations.FirstOrDefaultAsync(l=>l.Id == request.Id, cancellationToken);

        if (locationToDelete == null)
        {
            throw new FluentValidation.ValidationException(new[]
            {
                new ValidationFailure("Location",
                    $"Verilen {request.Id} numaralı lokasyon kayıtına ulaşılamadı. Id numarasını veya lokasyon bilgisinin varlığını kontrol edin.")
            });
        }
        _context.Locations.Remove(locationToDelete);
        await _context.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}