using OrderDeliveryService.Application.Abstractions.Persistence.Repositories;
using OrderDeliveryService.Application.Abstractions.Persistence.Requests;
using OrderDeliveryService.Application.Models.Orders;

namespace OrderDeliveryService.Infrastructure.Persistence.Repositories.Postgres;

public class OrderRepository : IOrderRepository
{
    public Task<long> CreateAsync(CreateOrderRepositoryRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<Order> GetAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}