using OrderDeliveryService.Application.Models.Orders;

namespace OrderDeliveryService.Application.Contracts.Orders.Operations;

public class CreateOrder
{
    public record Request(
        string SendersCity,
        string SendersAddress,
        string RecipientsCity,
        string RecipientsAddress,
        decimal CargoWeight,
        DateTimeOffset CargoCollectionDate);

    public abstract record Result
    {
        private Result() { }

        public sealed record Success(Order Order) : Result;
    }
}