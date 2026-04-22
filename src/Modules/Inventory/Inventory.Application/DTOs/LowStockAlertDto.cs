using System;

namespace Inventory.Application.DTOs
{
    public class LowStockAlertDto
    {
        public Guid ProductId { get; set; }
        public int AvailableStock { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
