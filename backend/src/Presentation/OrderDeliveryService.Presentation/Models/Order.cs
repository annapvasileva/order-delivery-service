namespace OrderDeliveryService.Presentation.Models;

public record Order(
    long OrderId,
    string SendersCity,
    string SendersAddress,
    string RecipientsCity,
    string RecipientsAddress,
    decimal CargoWeight,
    DateTimeOffset CargoCollectionDate);