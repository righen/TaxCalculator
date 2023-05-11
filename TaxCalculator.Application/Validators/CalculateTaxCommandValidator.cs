namespace TaxCalculator.Application.Validators;

using FluentValidation;
using TaxCalculator.Application.Commands;

public class CalculateTaxCommandValidator : AbstractValidator<CalculateTaxCommand>
{
    public CalculateTaxCommandValidator()
    {
        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .WithMessage("Postal code is required.");

        RuleFor(x => x.AnnualIncome)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Annual income must be greater than or equal to 0.");
    }
}
