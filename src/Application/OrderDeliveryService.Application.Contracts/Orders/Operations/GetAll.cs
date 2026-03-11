using OrderDeliveryService.Application.Models.Common;
using OrderDeliveryService.Application.Models.Orders;

namespace OrderDeliveryService.Application.Contracts.Orders.Operations;

public class GetAll
{
    public record Request(
        int PageSize,
        PageToken? PageToken = null);

    public abstract record Result
    {
        private Result() { }

        public sealed record Success(
            IList<Order> Orders,
            PageToken? PageToken) : Result;
    }
}