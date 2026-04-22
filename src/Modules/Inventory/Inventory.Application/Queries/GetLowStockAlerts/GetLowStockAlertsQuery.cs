using MediatR;
using Inventory.Application.DTOs;

namespace Inventory.Application.Queries.GetLowStockAlerts
{
    public record GetLowStockAlertsQuery(int Threshold = 5) : IRequest<List<LowStockAlertDto>>;
}
