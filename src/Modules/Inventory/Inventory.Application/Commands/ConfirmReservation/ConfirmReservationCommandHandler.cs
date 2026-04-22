using Inventory.Application.Commands.ConfirmReservation;
using Inventory.Application.Interfaces;
using MediatR;

namespace Inventory.Application.Handlers
{
    public class ConfirmReservationCommandHandler : IRequestHandler<ConfirmReservationCommand, bool>
    {
        private readonly IInventoryRepository _repo;

        public ConfirmReservationCommandHandler(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(ConfirmReservationCommand request, CancellationToken ct)
        {
            var inventory = await _repo.GetByProductIdAsync(request.ProductId);

            if (inventory == null)
                return false;

            try
            {
                inventory.ConfirmReservation(request.Quantity);
            }
            catch (InvalidOperationException)
            {
                return false;
            }

            await _repo.UpdateAsync(inventory);
            await _repo.SaveChangesAsync(ct);

            return true;
        }
    }
}
