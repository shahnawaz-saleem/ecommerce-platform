using System;
using System.Collections.Generic;

namespace Inventory.Application.DTOs
{
    public class RecentRestockDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime? LastRestockedAt { get; set; }
    }

    public class DashboardDto
    {
        public int TotalProducts { get; set; }
        public int LowStockItems { get; set; }
        public int OutOfStockItems { get; set; }
        public IEnumerable<RecentRestockDto> RecentRestocks { get; set; } = new List<RecentRestockDto>();
    }
}
