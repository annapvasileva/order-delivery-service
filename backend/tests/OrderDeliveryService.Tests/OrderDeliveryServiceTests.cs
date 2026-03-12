using Moq;
using OrderDeliveryService.Application.Abstractions.Persistence.Repositories;
using OrderDeliveryService.Application.Abstractions.Persistence.Requests;
using OrderDeliveryService.Application.Contracts.Orders.Operations;
using OrderDeliveryService.Application.Models.Common;
using OrderDeliveryService.Application.Models.Orders;
using OrderService = OrderDeliveryService.Application.Orders.OrderDeliveryService;

namespace OrderDeliveryService.Tests;

public class OrderDeliveryServiceTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly OrderService _service;

    public OrderDeliveryServiceTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _service = new OrderService(_orderRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateOrderAsync_ValidOrder_CreateOrderAndReturnSuccess()
    {
        // Arrange
        var request = new CreateOrder.Request(
            "Moscow",
            "Lenina 1",
            "Saint Petersburg",
            "Nevsky 10",
            10.5m,
            DateTime.UtcNow);

        long createdOrderId = 239;

        _orderRepositoryMock
            .Setup(r => r.CreateAsync(
                It.IsAny<CreateOrderRepositoryRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdOrderId);

        // Act
        var result = await _service.CreateOrderAsync(request, CancellationToken.None);

        // Assert
        var success = Assert.IsType<CreateOrder.Result.Success>(result);

        Assert.Equal(createdOrderId, success.Order.OrderId);
        Assert.Equal(request.SendersCity, success.Order.SendersCity);
        Assert.Equal(request.SendersAddress, success.Order.SendersAddress);
        Assert.Equal(request.RecipientsCity, success.Order.RecipientsCity);
        Assert.Equal(request.RecipientsAddress, success.Order.RecipientsAddress);
        Assert.Equal(request.CargoWeight, success.Order.CargoWeight);
        Assert.Equal(request.CargoCollectionDate, success.Order.CargoCollectionDate);

        _orderRepositoryMock.Verify(r => r.CreateAsync(
                It.Is<CreateOrderRepositoryRequest>(x =>
                    x.SendersCity == request.SendersCity &&
                    x.SendersAddress == request.SendersAddress &&
                    x.RecipientsCity == request.RecipientsCity &&
                    x.RecipientsAddress == request.RecipientsAddress &&
                    x.CargoWeight == request.CargoWeight &&
                    x.CargoCollectionDate == request.CargoCollectionDate),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ItemsCountLessThanPageSize_ReturnPageWithoutPageToken()
    {
        // Arrange
        var request = new GetAll.Request(
            PageSize: 5,
            PageToken: null);

        var orders = new List<Order>
        {
            new() { OrderId = 1 },
            new() { OrderId = 2 },
            new() { OrderId = 3 }
        };

        _orderRepositoryMock
            .Setup(r => r.GetAllAsync(
                It.IsAny<GetAllOrdersRepositoryRequest>(),
                It.IsAny<CancellationToken>()))
            .Returns(orders.ToAsyncEnumerable());

        // Act
        var result = await _service.GetAllAsync(request, CancellationToken.None);

        // Assert
        var success = Assert.IsType<GetAll.Result.Success>(result);

        Assert.Equal(3, success.Orders.Count);
        Assert.Null(success.PageToken);
    }

    [Fact]
    public async Task GetAllAsync_ItemsCountEqualsToPageSize_ShouldReturnNextPageToken()
    {
        // Arrange
        var request = new GetAll.Request(
            PageSize: 3,
            PageToken: null);

        var orders = new List<Order>
        {
            new() { OrderId = 10 },
            new() { OrderId = 11 },
            new() { OrderId = 12 }
        };

        _orderRepositoryMock
            .Setup(r => r.GetAllAsync(
                It.IsAny<GetAllOrdersRepositoryRequest>(),
                It.IsAny<CancellationToken>()))
            .Returns(orders.ToAsyncEnumerable());

        // Act
        var result = await _service.GetAllAsync(request, CancellationToken.None);

        // Assert
        var success = Assert.IsType<GetAll.Result.Success>(result);

        Assert.Equal(3, success.Orders.Count);
        Assert.NotNull(success.PageToken);
        Assert.Equal(12, success.PageToken.LastSeenId);
    }

    [Fact]
    public async Task GetAllAsync_NoItemsFoundForGivenPageToken_ShouldReturnEmptyListWithNoPageToken()
    {
        // Arrange
        var request = new GetAll.Request(
            PageSize: 3,
            PageToken: new PageToken(12));

        var orders = new List<Order>();

        _orderRepositoryMock
            .Setup(r => r.GetAllAsync(
                It.IsAny<GetAllOrdersRepositoryRequest>(),
                It.IsAny<CancellationToken>()))
            .Returns(orders.ToAsyncEnumerable());

        // Act
        var result = await _service.GetAllAsync(request, CancellationToken.None);

        // Assert
        var success = Assert.IsType<GetAll.Result.Success>(result);

        Assert.Empty(success.Orders);
        Assert.Null(success.PageToken);
    }
}