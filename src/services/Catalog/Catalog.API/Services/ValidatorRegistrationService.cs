using System.Reflection;
using Catalog.Application.Features.Products.Commands.CreateProduct.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;
namespace Catalog.API.Services;

public static class ValidatorRegistrationService
{
    public static IMvcBuilder AddFluentValidationValidators(this IMvcBuilder builder)
    {
        return builder.AddFluentValidation(fv =>
        {
            fv.RegisterValidatorsFromAssemblyContaining<CreateProductCommandValidator>();
        });
    }
}