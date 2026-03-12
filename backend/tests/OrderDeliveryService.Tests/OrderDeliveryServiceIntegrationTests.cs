using OrderDeliveryService.Application.Contracts.Orders.Operations;
using OrderDeliveryService.Infrastructure.Persistence.Repositories.Postgres;
using OrderService = OrderDeliveryService.Application.Orders.OrderDeliveryService;

namespace OrderDeliveryService.Tests;

public class OrderDeliveryServiceIntegrationTests : IClassFixture<PostgreSqlFixture>
{
    private readonly PostgreSqlFixture _fixture;

    public OrderDeliveryServiceIntegrationTests(PostgreSqlFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task CreateOrderAsync_ShouldPersistOrder()
    {
        // Arrange
        await using var context = DbContextFactory.Create(_fixture.ConnectionString);

        var repository = new OrderRepository(context);
        var service = new OrderService(repository);

        DateTimeOffset date = DateTimeOffset.UtcNow;
        var request = new CreateOrder.Request(
            "Helsinki",
            "Street 1",
            "Espoo",
            "Street 2",
            12.5m,
            date);

        // Act
        var result = await service.CreateOrderAsync(request, CancellationToken.None);

        // Assert
        var success = Assert.IsType<CreateOrder.Result.Success>(result);

        Assert.True(success.Order.OrderId > 0);
        Assert.Equal("Helsinki", success.Order.SendersCity);
        Assert.Equal("Street 1", success.Order.SendersAddress);
        Assert.Equal("Espoo", success.Order.RecipientsCity);
        Assert.Equal("Street 2", success.Order.RecipientsAddress);
        Assert.Equal(12.5m, success.Order.CargoWeight);
        Assert.Equal(date, success.Order.CargoCollectionDate);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnCreatedOrders()
    {
        // Arrange
        await using var context = DbContextFactory.Create(_fixture.ConnectionString);

        var repository = new OrderRepository(context);
        var service = new OrderService(repository);

        for (int i = 0; i < 3; i++)
        {
            var request = new CreateOrder.Request(
                "CityA",
                "AddrA",
                "CityB",
                "AddrB",
                5,
                DateTime.UtcNow);

            await service.CreateOrderAsync(request, CancellationToken.None);
        }

        var getRequest = new GetAll.Request(10, null);

        // Act
        var result = await service.GetAllAsync(getRequest, CancellationToken.None);

        // Assert
        var success = Assert.IsType<GetAll.Result.Success>(result);

        Assert.True(success.Orders.Count >= 3);
    }

    [Fact]
    public async Task GetAllAsync_ItemsCountEqualsPageSize_ReturnItemsAndPageToken()
    {
        // Arrange
        await using var context = DbContextFactory.Create(_fixture.ConnectionString);

        var repository = new OrderRepository(context);
        var service = new OrderService(repository);

        for (int i = 0; i < 5; i++)
        {
            var request = new CreateOrder.Request(
                "City",
                "Addr",
                "City2",
                "Addr2",
                1,
                DateTime.UtcNow);

            await service.CreateOrderAsync(request, CancellationToken.None);
        }

        var getRequest = new GetAll.Request(5, null);

        // Act
        var result = await service.GetAllAsync(getRequest, CancellationToken.None);

        // Assert
        var success = Assert.IsType<GetAll.Result.Success>(result);

        Assert.Equal(5, success.Orders.Count);
        Assert.NotNull(success.PageToken);
    }
}