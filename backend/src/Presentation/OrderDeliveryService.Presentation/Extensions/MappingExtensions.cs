using OrderDeliveryService.Application.Models.Orders;
using PresentationOrder = OrderDeliveryService.Presentation.Models.Order;

namespace OrderDeliveryService.Presentation.Extensions;

public static class MappingExtensions
{
    public static PresentationOrder ToPresentation(this Order order)
    {
        return new PresentationOrder(
            order.OrderId,
            order.SendersCity,
            order.SendersAddress,
            order.RecipientsCity,
            order.RecipientsAddress,
            order.CargoWeight,
            order.CargoCollectionDate);
    }
}