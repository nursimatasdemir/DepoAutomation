using FluentValidation.AspNetCore;
using Identity.Application.Features.Auth.Commands.Register.Validation;

namespace Identity.API.Services;

public static class ValidatorRegistrationService
{
    public static IMvcBuilder AddFluentValidationValidators(this IMvcBuilder builder)
    {
        return builder.AddFluentValidation(fv =>
        {
            fv.RegisterValidatorsFromAssemblyContaining<RegisterCommandValidator>();
        });
    }
}