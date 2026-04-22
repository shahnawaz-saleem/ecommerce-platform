using FluentValidation;

namespace Inventory.Application.Commands.UpdateInventoryItem;

public class UpdateInventoryItemCommandValidator : AbstractValidator<UpdateInventoryItemCommand>
{
    public UpdateInventoryItemCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0);
    }
}
