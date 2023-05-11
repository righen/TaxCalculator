namespace TaxCalculator.Application.Commands;

using FluentValidation;
using MediatR;
using TaxCalculator.Application.Abstractions;

public class CalculateTaxCommand : IRequest<decimal>
{
    public string PostalCode { get; set; }
    public decimal AnnualIncome { get; set; }

    public CalculateTaxCommand(string postalCode, decimal annualIncome)
    {
        PostalCode = postalCode;
        AnnualIncome = annualIncome;
    }
}

public class CalculateTaxCommandHandler : IRequestHandler<CalculateTaxCommand, decimal>
{
    private readonly ITaxCalculatorFactory _taxCalculatorFactory;
    private readonly IValidator<CalculateTaxCommand> _validator;


    public CalculateTaxCommandHandler(ITaxCalculatorFactory taxCalculatorFactory, IValidator<CalculateTaxCommand> validator)
    {
        _taxCalculatorFactory = taxCalculatorFactory;
        _validator = validator;
    }

    public Task<decimal> Handle(CalculateTaxCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        
        if (request.AnnualIncome == 0)
            return Task.FromResult(decimal.Zero);
        
        var taxCalculator = _taxCalculatorFactory.Create(request.PostalCode);

        if (taxCalculator == null)
        {
            throw new ArgumentException($"No tax calculator found for postal code: {request.PostalCode}");
        }
        
        var taxAmount = taxCalculator.CalculateTax(request.AnnualIncome);
        return Task.FromResult(taxAmount);
    }
}