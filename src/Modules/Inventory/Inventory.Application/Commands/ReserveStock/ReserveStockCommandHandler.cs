using Inventory.Application.Commands.ReserveStock;
using Inventory.Application.Interfaces;
using MediatR;

namespace Inventory.Application.Handlers
{
    public class ReserveStockCommandHandler : IRequestHandler<ReserveStockCommand, bool>
    {
        private readonly IInventoryRepository _repo;

        public ReserveStockCommandHandler(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(ReserveStockCommand request, CancellationToken ct)
        {
            var inventory = await _repo.GetByProductIdAsync(request.ProductId);

            if (inventory == null)
                return false;

            try
            {
                inventory.ReserveStock(request.Quantity);
            }
            catch (InvalidOperationException)
            {
                return false; // insufficient stock
            }

            await _repo.UpdateAsync(inventory);
            await _repo.SaveChangesAsync(ct);

            return true;
        }
    }
}
