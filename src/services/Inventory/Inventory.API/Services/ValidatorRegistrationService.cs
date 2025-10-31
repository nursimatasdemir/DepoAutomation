using FluentValidation.AspNetCore;
using Inventory.Application.Features.StockTransactions.Commands.ReceiveStock.Validation;

namespace Catalog.API.Services;

public static class ValidatorRegistrationService
{
    public static IMvcBuilder AddFluentValidationValidators(this IMvcBuilder builder)
    {
        return builder.AddFluentValidation(fv =>
        {
            fv.RegisterValidatorsFromAssemblyContaining<ReceiveStockCommandValidator>();
        });
    }
}