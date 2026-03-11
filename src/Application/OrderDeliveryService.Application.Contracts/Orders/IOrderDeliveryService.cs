using OrderDeliveryService.Application.Contracts.Orders.Operations;

namespace OrderDeliveryService.Application.Contracts.Orders;

public interface IOrderDeliveryService
{
    Task<CreateOrder.Result> CreateOrderAsync(
        CreateOrder.Request request,
        CancellationToken cancellationToken);
    
    Task<GetAll.Result> GetAllAsync(
        GetAll.Request request,
        CancellationToken cancellationToken);
}