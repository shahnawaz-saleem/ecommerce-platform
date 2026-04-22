using MediatR;
using Inventory.Application.DTOs;

namespace Inventory.Application.Queries.GetInventoryDashboard
{
    public record GetInventoryDashboardQuery() : IRequest<DashboardDto>;
}
