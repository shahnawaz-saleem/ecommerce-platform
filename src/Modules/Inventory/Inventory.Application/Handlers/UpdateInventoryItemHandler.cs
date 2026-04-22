using Inventory.Application.Commands.UpdateInventoryItem;
using Inventory.Application.Interfaces;
using MediatR;

namespace Inventory.Application.Handlers
{
    public class UpdateInventoryItemHandler : IRequestHandler<UpdateInventoryItemCommand, bool>
    {
        private readonly IInventoryRepository _repository;

        public UpdateInventoryItemHandler(IInventoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(UpdateInventoryItemCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByIdAsync(request.Id);
            if (existing == null)
                return false;

            // Adjust total stock to match requested quantity
            var currentTotal = existing.TotalStock;
            var desiredTotal = request.Quantity;

            if (desiredTotal > currentTotal)
            {
                existing.AddStock(desiredTotal - currentTotal);
            }
            else if (desiredTotal < currentTotal)
            {
                existing.ReduceStock(currentTotal - desiredTotal);
            }

            if (request.IsDeleted)
                existing.Delete();

            await _repository.UpdateAsync(existing);

            return true;
        }
    }
}
