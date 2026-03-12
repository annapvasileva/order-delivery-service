using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using OrderDeliveryService.Application.Abstractions.Persistence.Repositories;
using OrderDeliveryService.Application.Abstractions.Persistence.Requests;
using OrderDeliveryService.Application.Models.Orders;
using OrderDeliveryService.Infrastructure.Persistence.DbContexts;

namespace OrderDeliveryService.Infrastructure.Persistence.Repositories.Postgres;

public class OrderRepository : IOrderRepository
{
    private readonly PostgresDbContext _context;

    public OrderRepository(PostgresDbContext context)
    {
        _context = context;
    }

    public async Task<long> CreateAsync(
        CreateOrderRepositoryRequest request,
        CancellationToken cancellationToken)
    {
        var order = new Order
        {
            SendersCity = request.SendersCity,
            SendersAddress = request.SendersAddress,
            RecipientsCity = request.RecipientsCity,
            RecipientsAddress = request.RecipientsAddress,
            CargoWeight = request.CargoWeight,
            CargoCollectionDate = request.CargoCollectionDate
        };

        await _context.Orders.AddAsync(order, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);

        return order.OrderId;
    }

    public async IAsyncEnumerable<Order> GetAllAsync(
        GetAllOrdersRepositoryRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        IQueryable<Order> query = _context.Orders
            .AsNoTracking()
            .OrderBy(x => x.OrderId);

        if (request.LastSeenId is not null)
        {
            query = query.Where(x => x.OrderId > request.LastSeenId);
        }

        query = query.Take(request.PageSize);

        await foreach (Order order in query
                           .AsAsyncEnumerable()
                           .WithCancellation(cancellationToken))
        {
            yield return order;
        }
    }
}