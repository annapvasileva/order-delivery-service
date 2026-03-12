namespace OrderDeliveryService.Presentation.Models.Operations.Requests;

public record CreateOrderRequest(
    string SendersCity,
    string SendersAddress,
    string RecipientsCity,
    string RecipientsAddress,
    decimal CargoWeight,
    DateTimeOffset CargoCollectionDate);