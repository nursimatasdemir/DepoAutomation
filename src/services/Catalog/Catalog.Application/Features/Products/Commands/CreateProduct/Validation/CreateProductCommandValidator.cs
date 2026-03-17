using FluentValidation;
using Catalog.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Features.Products.Commands.CreateProduct.Validation;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public readonly IApplicationDbContext _context;
    
    public CreateProductCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        
        RuleFor(command => command.Name).NotEmpty().WithMessage("Ürün adı boş olamaz!");
        
        RuleFor(command => command.Sku).NotEmpty().WithMessage("SKU (Stok Kodu) boş olamaz!")
            .MinimumLength(3).WithMessage("SKU (Stok Kodu) en az 3 karakterden oluşmalıdır!");
        
        RuleFor(command => command.CategoryId).NotEmpty().WithMessage("Kategori ID boş bırakılamaz!");

        RuleFor(command => command.CategoryId).MustAsync(async (categoryId, cancellationToken) =>
        {
            return await _context.Categories.AnyAsync(c => c.Id == categoryId, cancellationToken);
        })
        .WithMessage(command => $"Verilen {command.CategoryId} ile kayıtlı Kategori Id numarası bulunamadı!")
        .OverridePropertyName("Category");
        
        RuleFor(command => command.Sku).MustAsync(async (Sku, cancellationToken) =>
        {
            return ! await _context.Products.AnyAsync(p => p.Sku == Sku, cancellationToken);
        })
        .WithMessage(command => $"Stok kodu {command.Sku} zaten başka bir ürün tarafından kullanılıyor")
        .OverridePropertyName("SKU");
        
        RuleFor(command => command.Barcode).MustAsync(async (Barcode, cancellationToken) =>
            {
                return ! await _context.Products.AnyAsync(p => p.Barcode == Barcode, cancellationToken);
            })
            .WithMessage(command => $"Barkod Numarası : {command.Barcode} başka bir ürünü temsil etmektedir")
            .OverridePropertyName("Barcode");
    }
    
}