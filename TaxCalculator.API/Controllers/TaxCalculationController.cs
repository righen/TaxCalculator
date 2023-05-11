namespace TaxCalculator.Controllers;

using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaxCalculator.Application.Commands;

[ApiController]
[Route("[controller]")]
public class TaxCalculatorController : ControllerBase
{
    private readonly IMediator _mediator;

    public TaxCalculatorController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CalculateTax(decimal annualIncome, string postalCode)
    {
        var calculateTaxCommand = new CalculateTaxCommand(postalCode, annualIncome);
        var result = await _mediator.Send(calculateTaxCommand);
        return Ok(result);
    }
}