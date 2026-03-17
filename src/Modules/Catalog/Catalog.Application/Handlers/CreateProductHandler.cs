using Catalog.Application.Interfaces;
using Catalog.Application.Products.Commands.CreateProduct;
using Catalog.Domain.Entities;
using MediatR;

namespace Catalog.Application.Handlers;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _productRepository;

    public CreateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product(
            request.Name,
            request.Description,
            request.Price,
            request.CategoryId,
            request.VendorId,
            request.StockQuantity
        );

        await _productRepository.AddAsync(product, cancellationToken);

        await _productRepository.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}