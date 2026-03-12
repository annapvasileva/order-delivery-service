using OrderDeliveryService.Application.Abstractions.Persistence.Repositories;
using OrderDeliveryService.Application.Abstractions.Persistence.Requests;
using OrderDeliveryService.Application.Contracts.Orders;
using OrderDeliveryService.Application.Contracts.Orders.Operations;
using OrderDeliveryService.Application.Models.Common;
using OrderDeliveryService.Application.Models.Orders;

namespace OrderDeliveryService.Application.Orders;

public class OrderDeliveryService : IOrderDeliveryService
{
    private readonly IOrderRepository _orderRepository;

    public OrderDeliveryService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<CreateOrder.Result> CreateOrderAsync(CreateOrder.Request request, CancellationToken cancellationToken)
    {
        var createOrderRepositoryRequest = new CreateOrderRepositoryRequest(
            request.SendersCity,
            request.SendersAddress,
            request.RecipientsCity,
            request.RecipientsAddress,
            request.CargoWeight,
            request.CargoCollectionDate);

        long orderId = await _orderRepository.CreateAsync(createOrderRepositoryRequest, cancellationToken);

        var order = new Order
        {
            OrderId = orderId,
            SendersCity = request.SendersCity,
            SendersAddress = request.SendersAddress,
            RecipientsCity = request.RecipientsCity,
            RecipientsAddress = request.RecipientsAddress,
            CargoWeight = request.CargoWeight,
            CargoCollectionDate = request.CargoCollectionDate,
        };

        return new CreateOrder.Result.Success(order);
    }

    public async Task<GetAll.Result> GetAllAsync(GetAll.Request request, CancellationToken cancellationToken)
    {
        long? lastSeenId = request.PageToken?.LastSeenId;
        var repositoryRequest = new GetAllOrdersRepositoryRequest(
            request.PageSize,
            lastSeenId);
        List<Order> page = await _orderRepository
            .GetAllAsync(repositoryRequest, cancellationToken)
            .ToListAsync(cancellationToken);

        if (page.Count < request.PageSize)
        {
            return new GetAll.Result.Success(page, null);
        }

        long lastSeenIdToReturn = page[^1].OrderId;

        return new GetAll.Result.Success(page, new PageToken(lastSeenIdToReturn));
    }
}