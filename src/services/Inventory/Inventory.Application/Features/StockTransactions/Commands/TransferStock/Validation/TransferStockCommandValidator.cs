using FluentValidation;

namespace Inventory.Application.Features.StockTransactions.Commands.TransferStock.Validation;

public class TransferStockCommandValidator : AbstractValidator<TransferStockCommand>
{
    public TransferStockCommandValidator()
    {
        RuleFor(command => command.ProductId).NotEmpty().WithMessage("Ürün Id kısmı boş bırakılamaz");
        RuleFor(command => command.SourceLocationId).NotEmpty().WithMessage("Kaynak Lokasyon ID bilgisi boş olamaz");
        RuleFor(command => command.DestinationLocationId).NotEmpty().WithMessage("Hedef Lokasyon ID bilgisi boş olamaz");

        RuleFor(command => command.QuantityToTransfer).GreaterThan(0)
            .WithMessage("Trnasfer edilecek ürün miktarı 0'dan büyük olmalıdır.");

        RuleFor(c => c).Must(c => c.SourceLocationId != c.DestinationLocationId)
            .WithMessage("Kaynak ve hedef lokasyon aynı olamaz.");
    }
}