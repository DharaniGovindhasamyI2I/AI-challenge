using FluentValidation;

namespace CleanArchitecture.Application.Products.Commands.UpdateInventory
{
    public class UpdateInventoryCommandValidator : AbstractValidator<UpdateInventoryCommand>
    {
        public UpdateInventoryCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("ProductId must be greater than zero.");
            RuleFor(x => x.NewInventory)
                .GreaterThanOrEqualTo(0).WithMessage("Inventory cannot be negative.");
        }
    }
} 