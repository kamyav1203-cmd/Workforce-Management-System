using FluentValidation;
using WMS.Application.DTOs;

namespace WMS.Application.Validators;

public class CreateEmployeeDtoValidator : AbstractValidator<CreateEmployeeDto>
{
    public CreateEmployeeDtoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(80);
        RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(15);
        RuleFor(x => x.Gender).Must(g => g is "M" or "F" or "O");
        RuleFor(x => x.DOB).LessThan(DateTime.Today.AddYears(-18)).WithMessage("Employee must be at least 18 years old.");
        RuleFor(x => x.DOJ).LessThanOrEqualTo(DateTime.Today);
        RuleFor(x => x.DepartmentId).GreaterThan(0);
        RuleFor(x => x.RoleId).GreaterThan(0);
    }
}
