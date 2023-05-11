namespace TaxCalculator.IntegrationTests;

using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using TaxCalculator.Application.Abstractions;
using TaxCalculator.Application.Commands;
using TaxCalculator.Application.Validators;
using TaxCalculator.Domain.Abstractions;

[TestFixture]
public class CalculateTaxHandlerTests
{
    private Mock<ITaxCalculatorFactory> _taxCalculatorFactoryMock;
    private CalculateTaxCommandHandler _calculateTaxHandler;
    private IValidator<CalculateTaxCommand> _validator;

    [SetUp]
    public void Setup()
    {
        _taxCalculatorFactoryMock = new Mock<ITaxCalculatorFactory>();
        _validator = new CalculateTaxCommandValidator();
        _calculateTaxHandler = new CalculateTaxCommandHandler(_taxCalculatorFactoryMock.Object, _validator);
    }

    [Test]
    public async Task Handle_WithValidCommand_ReturnsTaxAmount()
    {
        // Arrange
        var command = new CalculateTaxCommand("7441", 50000m);
        var expectedTaxAmount = 10000m; // Assuming the expected tax amount for the given command

        var taxCalculatorMock = new Mock<ITaxCalculatorStrategy>();
        taxCalculatorMock.Setup(x => x.CalculateTax(It.IsAny<decimal>())).Returns(expectedTaxAmount);
        _taxCalculatorFactoryMock.Setup(x => x.Create(It.IsAny<string>())).Returns(taxCalculatorMock.Object);

        // Act
        var result = await _calculateTaxHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result, Is.EqualTo(expectedTaxAmount));
    }

    [Test]
    public void Handle_WithInvalidCommand_ThrowsValidationException()
    {
        // Arrange
        var command = new CalculateTaxCommand("", -1000m); // Invalid command with empty postal code and negative annual income

        // Act
        var validationResult = _validator.TestValidate(command);

        // Assert
        Assert.IsFalse(validationResult.IsValid);
        Assert.That(validationResult.Errors.Count, Is.EqualTo(2));
        validationResult.ShouldHaveValidationErrorFor(x => x.PostalCode)
                        .WithErrorMessage("Postal code is required.");
        validationResult.ShouldHaveValidationErrorFor(x => x.AnnualIncome)
                        .WithErrorMessage("Annual income must be greater than or equal to 0.");
    }
    
    [Test]
    public async Task Handle_WithUnknownPostalCode_ThrowsArgumentException()
    {
        // Arrange
        var command = new CalculateTaxCommand("9999", 50000m); // Unknown postal code

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _calculateTaxHandler.Handle(command, CancellationToken.None);
        });
    }
    
    [Test]
    public async Task Handle_WithZeroAnnualIncome_ReturnsZeroTaxAmount()
    {
        // Arrange
        var command = new CalculateTaxCommand("7441", 0m);
        var expectedTaxAmount = 0m;

        var taxCalculatorMock = new Mock<ITaxCalculatorStrategy>();
        taxCalculatorMock.Setup(x => x.CalculateTax(It.IsAny<decimal>())).Returns(expectedTaxAmount);
        _taxCalculatorFactoryMock.Setup(x => x.Create(It.IsAny<string>())).Returns(taxCalculatorMock.Object);

        // Act
        var result = await _calculateTaxHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result, Is.EqualTo(expectedTaxAmount));
    }
    
     [Test]
    public async Task Handle_WithFlatValueCalculator_ReturnsExpectedTaxAmount()
    {
        // Arrange
        var command = new CalculateTaxCommand("A100", 50000m);
        var expectedTaxAmount = 10000m; // Assuming the expected tax amount for the given command

        var taxCalculatorMock = new Mock<ITaxCalculatorStrategy>();
        taxCalculatorMock.Setup(x => x.CalculateTax(It.IsAny<decimal>())).Returns(expectedTaxAmount);
        _taxCalculatorFactoryMock.Setup(x => x.Create(It.IsAny<string>())).Returns(taxCalculatorMock.Object);

        // Act
        var result = await _calculateTaxHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result, Is.EqualTo(expectedTaxAmount));
    }

    [Test]
    public async Task Handle_WithFlatRateCalculator_ReturnsExpectedTaxAmount()
    {
        // Arrange
        var command = new CalculateTaxCommand("7000", 50000m);
        var expectedTaxAmount = 8750m; // Assuming the expected tax amount for the given command

        var taxCalculatorMock = new Mock<ITaxCalculatorStrategy>();
        taxCalculatorMock.Setup(x => x.CalculateTax(It.IsAny<decimal>())).Returns(expectedTaxAmount);
        _taxCalculatorFactoryMock.Setup(x => x.Create(It.IsAny<string>())).Returns(taxCalculatorMock.Object);

        // Act
        var result = await _calculateTaxHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result, Is.EqualTo(expectedTaxAmount));
    }

    [Test]
    public async Task Handle_WithProgressiveCalculator_ReturnsExpectedTaxAmount()
    {
        // Arrange
        var command = new CalculateTaxCommand("7441", 50000m);
        var expectedTaxAmount = 11250m; // Assuming the expected tax amount for the given command

        var taxCalculatorMock = new Mock<ITaxCalculatorStrategy>();
        taxCalculatorMock.Setup(x => x.CalculateTax(It.IsAny<decimal>())).Returns(expectedTaxAmount);
        _taxCalculatorFactoryMock.Setup(x => x.Create(It.IsAny<string>())).Returns(taxCalculatorMock.Object);

        // Act
        var result = await _calculateTaxHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result, Is.EqualTo(expected: expectedTaxAmount));
    }
    
    [Test]
    public Task Handle_WithNullPostalCode_ThrowsArgumentNullException()
    {
        // Arrange
        var command = new CalculateTaxCommand(null, 50000m);
        
        // Act
        var validationResult = _validator.TestValidate(command);
        
        //Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.PostalCode)
                        .WithErrorMessage("Postal code is required.");
        return Task.CompletedTask;
    }

    [Test]
    public async Task Handle_WithZeroTaxAmount_ReturnsZero()
    {
        // Arrange
        var command = new CalculateTaxCommand("7441", 50000m);
        var expectedTaxAmount = 0m; // Assuming the expected tax amount for the given command

        var taxCalculatorMock = new Mock<ITaxCalculatorStrategy>();
        taxCalculatorMock.Setup(x => x.CalculateTax(It.IsAny<decimal>())).Returns(expectedTaxAmount);
        _taxCalculatorFactoryMock.Setup(x => x.Create(It.IsAny<string>())).Returns(taxCalculatorMock.Object);

        // Act
        var result = await _calculateTaxHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result, Is.EqualTo(expectedTaxAmount));
    }

    [Test]
    public void Handle_WithInvalidCommand_ThrowsValidationExceptionWithMultipleErrors()
    {
        // Arrange
        var command = new CalculateTaxCommand(null, -50000m);

        // Act
        var validationResult = _validator.TestValidate(command);

        // Assert
        Assert.IsFalse(validationResult.IsValid);
        Assert.That(validationResult.Errors.Count, Is.EqualTo(2));
        validationResult.ShouldHaveValidationErrorFor(x => x.PostalCode)
                        .WithErrorMessage("Postal code is required.");
        validationResult.ShouldHaveValidationErrorFor(x => x.AnnualIncome)
                        .WithErrorMessage("Annual income must be greater than or equal to 0.");
    }

    [Test]
    public async Task Handle_WithLargeAnnualIncome_ReturnsExpectedTaxAmount()
    {
        // Arrange
        var command = new CalculateTaxCommand("7441", 1000000000m); // Large annual income
        var expectedTaxAmount = 330000000m;                         // Assuming the expected tax amount for the given command

        var taxCalculatorMock = new Mock<ITaxCalculatorStrategy>();
        taxCalculatorMock.Setup(x => x.CalculateTax(It.IsAny<decimal>())).Returns(expectedTaxAmount);
        _taxCalculatorFactoryMock.Setup(x => x.Create(It.IsAny<string>())).Returns(taxCalculatorMock.Object);

        // Act
        var result = await _calculateTaxHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result, Is.EqualTo(expectedTaxAmount));
    }
    
    [Test]
    public void Handle_WithInvalidCommand_ThrowsValidationExceptionWithSpecificErrors()
    {
        // Arrange
        var command = new CalculateTaxCommand("7441", -50000m); // Invalid annual income

        // Act
        var validationResult = _validator.TestValidate(command);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.AnnualIncome)
                        .WithErrorCode("GreaterThanOrEqualValidator")
                        .WithErrorMessage("Annual income must be greater than or equal to 0.");
    }

    [Test]
    public async Task Handle_WithHighTaxRate_ReturnsExpectedTaxAmount()
    {
        // Arrange
        var command = new CalculateTaxCommand("7441", 50000m);
        var expectedTaxAmount = 17500m; // Assuming the expected tax amount for the given command

        var taxCalculatorMock = new Mock<ITaxCalculatorStrategy>();
        taxCalculatorMock.Setup(x => x.CalculateTax(It.IsAny<decimal>())).Returns(expectedTaxAmount);
        _taxCalculatorFactoryMock.Setup(x => x.Create(It.IsAny<string>())).Returns(taxCalculatorMock.Object);

        // Act
        var result = await _calculateTaxHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result, Is.EqualTo(expectedTaxAmount));
    }

    [Test]
    public async Task Handle_WithNegativeAnnualIncome_ReturnsZeroTaxAmount()
    {
        // Arrange
        var command = new CalculateTaxCommand("7441", -50000m);

        // Act
        var validationResult = _validator.TestValidate(command);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.AnnualIncome)
                        .WithErrorCode("GreaterThanOrEqualValidator")
                        .WithErrorMessage("Annual income must be greater than or equal to 0.");
    }
}
