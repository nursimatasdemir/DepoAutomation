using Catalog.Application.Abstractions;
using Catalog.Domain;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    public readonly IApplicationDbContext _context;

    public CreateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var categoryExist = await _context.Categories.AnyAsync(c => c.Id == request.CategoryId, cancellationToken);
        if (!categoryExist)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure("Category", $"Verilen {request.CategoryId} ile kayıtlı Kategori ID numarası bulunamadı")
            });
        }
        
        var skuExists = await _context.Products.AnyAsync(c=> c.Sku == request.Sku, cancellationToken);
        if (skuExists)
        {
            throw new FluentValidation.ValidationException(new[]
            {
                new ValidationFailure("SKU",$"Bu SKU {request.Sku} zaten başka bir ürün tarafından kullanılıyor")
            });

        }
        var barcodeExists = await _context.Products.AnyAsync(c=> c.Barcode == request.Barcode, cancellationToken);
        if (barcodeExists)
        {
            throw new FluentValidation.ValidationException(new[]
            {
                new ValidationFailure("Barcode", $"Bu Barkod Numarası : {request.Barcode} zaten başka bir ürün tarafından kullanılıyor")
            });

        }
        var newProduct = new Product
        {
            Id = Guid.NewGuid(),
            Sku = request.Sku,
            Name = request.Name,
            Barcode = request.Barcode,
            CategoryId = request.CategoryId,
        };
        
        await _context.Products.AddAsync(newProduct, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return newProduct.Id;
    }
}