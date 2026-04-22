using MediatR;

namespace Inventory.Application.Commands.ReleaseReservation;

public record ReleaseReservationCommand(Guid ProductId, int Quantity) : IRequest<bool>;
