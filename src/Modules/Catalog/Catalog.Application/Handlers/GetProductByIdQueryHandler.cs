using Catalog.Application.Interfaces;
using Catalog.Application.Queries.GetProductById;
using Catalog.Domain.Entities;
using Ecommerce.API.Caching;
using MediatR;


namespace Catalog.Application.Handlers
{
   
    public class GetProductByIdQueryHandler
        : IRequestHandler<GetProductByIdQuery, Product?>
    {
        private readonly IProductRepository _repository;
        private readonly ICacheService _cache;

        public GetProductByIdQueryHandler(
            IProductRepository repository,
            ICacheService cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<Product> Handle(
            GetProductByIdQuery request,
            CancellationToken cancellationToken)
        {
            var cacheKey = $"product_{request.Id}";

            var cachedProduct = await _cache.GetAsync<Product>(cacheKey);

            if (cachedProduct != null)
                return cachedProduct;

            var product = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (product == null)
                return null;

            await _cache.SetAsync(cacheKey, product, TimeSpan.FromMinutes(10));

            return product;
        }
    }
}
