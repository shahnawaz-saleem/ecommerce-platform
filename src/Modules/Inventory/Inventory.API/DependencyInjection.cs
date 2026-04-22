using Inventory.Application;
using Inventory.Infrastructure;
using MediatR;
using Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.API;

public static class DependencyInjection
{
    public static IServiceCollection AddInventoryModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(IntegrationEvent).Assembly));

        services.AddInventoryInfrastructure(configuration);

        return services;
    }
}