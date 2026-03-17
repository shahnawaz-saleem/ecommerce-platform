using Catalog.Application.Interfaces;
using Catalog.Application.Products.Commands.DeleteProduct;
using Ecommerce.API.Caching;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers
{
    public class DeleteProductCommandHandler
     : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository _repository;
        private readonly ICacheService _cache;

        public DeleteProductCommandHandler(
            IProductRepository repository,
            ICacheService cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<bool> Handle(
            DeleteProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id);

            if (product == null)
                throw new Exception("Product not found");

            product.Delete();

            await _repository.SaveChangesAsync();

            // invalidate cache
            await _cache.RemoveAsync($"product_{request.Id}");
            await _cache.RemoveAsync("products");

            return true;
        }
    }
}
