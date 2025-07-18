using FluentValidation;

namespace CleanArchitecture.Application.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100);
            RuleFor(x => x.Description)
                .MaximumLength(500);
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");
            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Currency is required.");
            RuleFor(x => x.Inventory)
                .GreaterThanOrEqualTo(0).WithMessage("Inventory cannot be negative.");
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category is required.");
        }
    }
} 