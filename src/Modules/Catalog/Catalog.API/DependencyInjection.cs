using Catalog.Application;
using Catalog.Infrastructure;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Catalog.Application.Products.Commands.CreateProduct;

namespace Catalog.API;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly));

        services.AddCatalogInfrastructure(configuration);

        return services;
    }
}