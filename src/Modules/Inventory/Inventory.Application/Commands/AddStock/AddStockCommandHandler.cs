using Inventory.Application.Commands.AddStock;
using Inventory.Application.Interfaces;
using MediatR;

namespace Inventory.Application.Handlers
{
    public class AddStockCommandHandler : IRequestHandler<AddStockCommand, Guid>
    {
        private readonly IInventoryRepository _repo;

        public AddStockCommandHandler(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<Guid> Handle(AddStockCommand request, CancellationToken ct)
        {
            var inventory = await _repo.GetByProductIdAsync(request.ProductId);

            if (inventory == null)
            {
                // Create new inventory aggregate if not exists
                inventory = new Inventory.Domain.Entities.InventoryItem(request.ProductId);
                if (request.Quantity > 0)
                    inventory.AddStock(request.Quantity);

                await _repo.AddAsync(inventory);
            }
            else
            {
                inventory.AddStock(request.Quantity);
                await _repo.UpdateAsync(inventory);
            }

            await _repo.SaveChangesAsync(ct);

            return inventory.Id;
        }
    }
}
