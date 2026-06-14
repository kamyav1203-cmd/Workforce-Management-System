using FluentValidation;
using WMS.Application.DTOs;

namespace WMS.Application.Validators;

public class CreateClientDtoValidator : AbstractValidator<CreateClientDto>
{
    public CreateClientDtoValidator()
    {
        RuleFor(x => x.ClientName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ClientLocation).MaximumLength(20);
    }
}
