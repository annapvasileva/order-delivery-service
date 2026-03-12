namespace OrderDeliveryService.Presentation.Models.Operations.Responses;

public record GetAllResponse(
    IList<Order> Orders,
    long? LastSeenId);