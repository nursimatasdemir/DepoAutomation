using FluentValidation;
namespace Inventory.Application.Features.StockTransactions.Commands.ReceiveStock.Validation;

public class ReceiveStockCommandValidator : AbstractValidator<ReceiveStockCommand>
{
    public ReceiveStockCommandValidator()
    {
        RuleFor(command => command.ProductId).NotNull().WithMessage("Ürün Id kısmı boş bırakılamaz");
        RuleFor(command => command.QuantityReceived).GreaterThan(0)
            .WithMessage("Kabul edilen ürün miktarı 0'dan büyük olmalıdır.");
        RuleFor(command => command.LocationId).NotNull().WithMessage("Lokasyon Id kısmı boş bırakılmaz.");
        RuleFor(command => command.SourceDocument).NotEmpty().WithMessage("lütfen bu kısmı boş bırakmayınız");
    }
}