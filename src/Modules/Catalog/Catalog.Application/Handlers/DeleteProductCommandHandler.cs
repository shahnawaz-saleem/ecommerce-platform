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

        public DeleteProductCommandHandler(
            IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (product == null)
                return false;

            product.Delete();

            await _repository.SaveChangesAsync(cancellationToken);

            // cache invalidation handled by domain event handlers

            return true;
        }
    }
}
