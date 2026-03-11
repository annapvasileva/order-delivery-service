using Microsoft.Extensions.DependencyInjection;
using OrderDeliveryService.Application.Contracts.Orders;

namespace OrderDeliveryService.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(
        this IServiceCollection services)
    {
        services.AddScoped<IOrderDeliveryService, Orders.OrderDeliveryService>();

        return services;
    }
}