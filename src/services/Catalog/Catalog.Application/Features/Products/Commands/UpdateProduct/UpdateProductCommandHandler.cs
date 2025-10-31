using Catalog.Application.Abstractions;
using Catalog.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDTO?>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductDTO?> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var productExist = await _context.Products.AnyAsync(c => c.Id == request.Id, cancellationToken);
        if (!productExist)
        {
            throw new FluentValidation.ValidationException("Belirtilen Ürün ID ile eşleşen ürün bulunamadı.");
        }
        var categoryExist = await _context.Categories.AnyAsync(c => c.Id == request.CategoryId, cancellationToken);
        if (!categoryExist)
        {
            throw new FluentValidation.ValidationException("Belirtilen Kategori ID ile eşleşen kategori bulunamadı.");
        }
        
        var productToUpdate = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (productToUpdate == null)
        {
            return null;
        }
        
        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == request.CategoryId, cancellationToken);
        if (!categoryExists)
        {
           
        }
        
        productToUpdate.Sku = request.Sku;
        productToUpdate.Name = request.Name;
        productToUpdate.Barcode = request.Barcode;
        productToUpdate.CategoryId = request.CategoryId;
        
        await _context.SaveChangesAsync(cancellationToken);

        return new ProductDTO
        {
            Id = productToUpdate.Id,
            Sku = productToUpdate.Sku,
            Name = productToUpdate.Name,
            Barcode = productToUpdate.Barcode,
            CategoryName = productToUpdate.Category?.Name ?? string.Empty,
        };
    }
}