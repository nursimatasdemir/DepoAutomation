using Catalog.Application.Abstractions;
using Catalog.Application.DTOs;
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
        var locationExists = await _context.Locations.AnyAsync(x => x.Id == request.Id);
        if (!locationExists)
        {
            throw new FluentValidation.ValidationException(new []
            {
                new ValidationFailure("Location", $"Verilen {request.Id} numaralı lokasyon kayıtına ulaşılamadı.")
            });
        }
        var locationToDelete = await _context.Locations.FindAsync(request.Id);
        if (locationToDelete == null)
        {
            throw new FluentValidation.ValidationException(new[]
            {
                new ValidationFailure("LocationId",
                    $"Verilen {request.Id} numaralı Id ile kayıtlı lokasyon bulunmamadı")
            });
        }
        _context.Locations.Remove(locationToDelete);
        await _context.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}