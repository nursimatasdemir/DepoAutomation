using Catalog.Application.Abstractions;
using Catalog.Application.DTOs;
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
        var locationToDelete = await _context.Locations.FindAsync(request.Id);
        if (locationToDelete == null)
        {
            return false;
        }
        _context.Locations.Remove(locationToDelete);
        await _context.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}