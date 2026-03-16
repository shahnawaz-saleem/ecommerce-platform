using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public decimal Price { get; private set; }

        public Guid CategoryId { get; private set; }

        public Guid VendorId { get; private set; }

        public int StockQuantity { get; private set; }

        public DateTime CreatedAt { get; private set; }

        private Product() { } // Required by EF Core

        public Product(
            string name,
            string description,
            decimal price,
            Guid categoryId,
            Guid vendorId,
            int stockQuantity)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;
            CategoryId = categoryId;
            VendorId = vendorId;
            StockQuantity = stockQuantity;
            CreatedAt = DateTime.UtcNow;
        }
        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice <= 0)
                throw new ArgumentException("Price must be greater than zero");

            Price = newPrice;
        }

        public void AddStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be positive");

            StockQuantity += quantity;
        }

        public void ReduceStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be positive");

            if (StockQuantity < quantity)
                throw new InvalidOperationException("Insufficient stock");

            StockQuantity -= quantity;
        }
        public void Update(string name,string description,decimal price,Guid categoryId,int stockQuantity)
        {
            Name = name;
            Description = description;
            Price = price;
            CategoryId = categoryId;
            StockQuantity = stockQuantity;
        }
    }
}
