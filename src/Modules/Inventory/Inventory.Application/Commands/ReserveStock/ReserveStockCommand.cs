using MediatR;

namespace Inventory.Application.Commands.ReserveStock;

public record ReserveStockCommand(Guid ProductId, int Quantity) : IRequest<bool>;
