using FluentValidation;

namespace Inventory.Application.Features.StockTransactions.Commands.PickStock.Validation;

public class PickStockCommandValidator : AbstractValidator<PickStockCommand>
{
    public PickStockCommandValidator()
    {
        RuleFor(command => command.ProductId).NotEmpty().WithMessage("Ürün Id kısmı boş bırakılamaz");
        RuleFor(command => command.SourceLocationId).NotEmpty().WithMessage("Kaynak Lokasyon ID bilgisi boş olamaz");

        RuleFor(command => command.QuantityToPick).GreaterThan(0)
            .WithMessage("Toplanan ürün miktarı 0'dan büyük olmalıdır.");

    }
}