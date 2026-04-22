using Inventory.Application.Commands.ReleaseReservation;
using Inventory.Application.Interfaces;
using MediatR;

namespace Inventory.Application.Handlers
{
    public class ReleaseReservationCommandHandler : IRequestHandler<ReleaseReservationCommand, bool>
    {
        private readonly IInventoryRepository _repo;

        public ReleaseReservationCommandHandler(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(ReleaseReservationCommand request, CancellationToken ct)
        {
            var inventory = await _repo.GetByProductIdAsync(request.ProductId);

            if (inventory == null)
                return false;

            try
            {
                inventory.ReleaseReservation(request.Quantity);
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
