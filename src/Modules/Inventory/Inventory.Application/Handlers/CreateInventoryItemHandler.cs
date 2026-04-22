using Inventory.Application.Commands.CreateInventoryItem;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Handlers
{
    public class CreateInventoryItemHandler : IRequestHandler<CreateInventoryItemCommand, Guid>
    {
        private readonly IInventoryRepository _repository;

        public CreateInventoryItemHandler(IInventoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateInventoryItemCommand request, CancellationToken cancellationToken)
        {
            var item = new Inventory.Domain.Entities.InventoryItem(request.ProductId);

            if (request.Quantity > 0)
            {
                item.AddStock(request.Quantity);
            }

            await _repository.AddAsync(item);

            return item.Id;
        }
    }
}
