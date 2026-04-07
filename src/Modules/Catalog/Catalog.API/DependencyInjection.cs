using Catalog.Application;
using Catalog.Application.Products.Commands.CreateProduct;
using Catalog.Infrastructure;
using MediatR;
using Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.API;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly));
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(IntegrationEvent).Assembly));

        services.AddCatalogInfrastructure(configuration);

        return services;
    }
}