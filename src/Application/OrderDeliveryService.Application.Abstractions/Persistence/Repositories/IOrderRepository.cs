using OrderDeliveryService.Application.Abstractions.Persistence.Requests;
using OrderDeliveryService.Application.Models.Orders;

namespace OrderDeliveryService.Application.Abstractions.Persistence.Repositories;

public interface IOrderRepository
{
    Task<long> CreateAsync(
        CreateOrderRepositoryRequest request,
        CancellationToken cancellationToken);
    
    IAsyncEnumerable<Order> GetAllAsync(
        CancellationToken cancellationToken);
}