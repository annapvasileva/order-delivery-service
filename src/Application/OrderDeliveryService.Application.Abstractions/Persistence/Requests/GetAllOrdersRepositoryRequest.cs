namespace OrderDeliveryService.Application.Abstractions.Persistence.Requests;

public record GetAllOrdersRepositoryRequest(
    int PageSize,
    long? LastSeenId = null);