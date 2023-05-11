using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaxCalculator.Application.Abstractions;
using TaxCalculator.Application.Commands;
using TaxCalculator.Application.Factories;
using TaxCalculator.Application.Validators;
using TaxCalculator.Infrastructure.Persistence;
using TaxCalculator.Infrastructure.Persistence.Repositories;
using ITaxCalculatorFactory = TaxCalculator.Application.Abstractions.ITaxCalculatorFactory;

var builder = WebApplication.CreateBuilder(args);

// Load configuration files
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddDbContext<TaxCalculatorDbContext>(options =>
                                                          options.UseSqlite(builder.Configuration.GetConnectionString("TaxCalculatorDbContext")));
builder.Services.AddScoped<ITaxBracketRepository, TaxBracketRepository>();
builder.Services.AddScoped<IPostalCodeTaxCalculatorRepository, PostalCodeTaxCalculatorRepository>();
builder.Services.AddScoped<ITaxCalculatorFactory, TaxCalculatorFactory>();

//Add validator to command handler
builder.Services.AddValidatorsFromAssemblyContaining<CalculateTaxCommandValidator>();
builder.Services.AddTransient<IValidator<CalculateTaxCommand>, CalculateTaxCommandValidator>();

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddTransient<IRequestHandler<CalculateTaxCommand, decimal>, CalculateTaxCommandHandler>();

// Add Swagger services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Tax Calculator API", Version = "v1" });
});

var app = builder.Build();

app.UseHttpsRedirection();

// Add Swagger middleware to the request pipeline
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tax Calculator API v1");
});

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();