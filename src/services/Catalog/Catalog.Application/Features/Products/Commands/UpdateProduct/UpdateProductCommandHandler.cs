using Catalog.Application.Abstractions;
using Catalog.Application.DTOs;
using FluentValidation.Results;
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
        var productToUpdate = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (productToUpdate == null)
        {
            throw new FluentValidation.ValidationException(new[]
            {
                new ValidationFailure("Product","Belirtilen Ürün ID ile eşleşen ürün bulunamadı.")
            });
        }
        
        var categoryExist = await _context.Categories.FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);
        if (categoryExist == null)
        {
            throw new FluentValidation.ValidationException(new []
            {
                new ValidationFailure("Category","Belirtilen Kategori ID ile eşleşen kategori bulunamadı.")
            });
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