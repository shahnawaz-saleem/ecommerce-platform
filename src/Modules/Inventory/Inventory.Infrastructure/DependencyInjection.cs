
using Inventory.Application.Interfaces;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInventoryInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<InventoryDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("InventoryDb")));

        services.AddScoped<IInventoryRepository, InventoryRepository>();

        return services;
    }
}
