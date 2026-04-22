using MediatR;

namespace Inventory.Application.Commands.ConfirmReservation;

public record ConfirmReservationCommand(Guid ProductId, int Quantity) : IRequest<bool>;
