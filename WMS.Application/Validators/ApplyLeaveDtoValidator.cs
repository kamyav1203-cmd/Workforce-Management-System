using FluentValidation;
using WMS.Application.DTOs;

namespace WMS.Application.Validators;

public class ApplyLeaveDtoValidator : AbstractValidator<ApplyLeaveDto>
{
    public ApplyLeaveDtoValidator()
    {
        RuleFor(x => x.EmpId).GreaterThan(0);
        RuleFor(x => x.LeaveType).NotEmpty().Must(t => t is "Sick" or "Casual" or "Earned");
        RuleFor(x => x.FromDate).LessThanOrEqualTo(x => x.ToDate);
        RuleFor(x => x.Reason).MaximumLength(255);
    }
}
