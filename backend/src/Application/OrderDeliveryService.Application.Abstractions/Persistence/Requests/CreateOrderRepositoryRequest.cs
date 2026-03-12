namespace OrderDeliveryService.Application.Abstractions.Persistence.Requests;

public record CreateOrderRepositoryRequest(
    string SendersCity,
    string SendersAddress,
    string RecipientsCity,
    string RecipientsAddress,
    decimal CargoWeight,
    DateTimeOffset CargoCollectionDate);