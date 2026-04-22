using MediatR;

namespace Inventory.Application.Commands.AddStock;

public record AddStockCommand(Guid ProductId, int Quantity) : IRequest<Guid>;
