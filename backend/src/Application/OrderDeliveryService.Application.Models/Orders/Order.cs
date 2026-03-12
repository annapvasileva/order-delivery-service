namespace OrderDeliveryService.Application.Models.Orders;

public class Order
{
    public long OrderId { get; set; }

    public string SendersCity { get; set; }

    public string SendersAddress { get; set; }

    public string RecipientsCity { get; set; }

    public string RecipientsAddress { get; set; }

    public decimal CargoWeight { get; set; }

    public DateTimeOffset CargoCollectionDate { get; set; }
}