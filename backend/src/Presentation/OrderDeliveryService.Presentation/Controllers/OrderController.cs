using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderDeliveryService.Application.Contracts.Orders;
using OrderDeliveryService.Application.Contracts.Orders.Operations;
using OrderDeliveryService.Application.Models.Common;
using OrderDeliveryService.Presentation.Extensions;
using OrderDeliveryService.Presentation.Models;
using OrderDeliveryService.Presentation.Models.Operations.Requests;
using OrderDeliveryService.Presentation.Models.Operations.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderDeliveryService.Presentation.Controllers;

[ApiController]
[Route("/v1/orders")]
public class OrderController : ControllerBase
{
    private const int MinimalPageSize = 1;

    private readonly IOrderDeliveryService _orderDeliveryService;

    public OrderController(IOrderDeliveryService orderDeliveryService)
    {
        _orderDeliveryService = orderDeliveryService;
    }

    [HttpPost]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CreateOrderResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CreateOrderResponse>> CreateOrder(
        [FromBody] CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        var serviceRequest = new CreateOrder.Request(
            request.SendersCity,
            request.SendersAddress,
            request.RecipientsCity,
            request.RecipientsAddress,
            request.CargoWeight,
            request.CargoCollectionDate);

        CreateOrder.Result serviceResult = await _orderDeliveryService.CreateOrderAsync(
            serviceRequest,
            cancellationToken);

        return serviceResult switch
        {
            CreateOrder.Result.Success success => Ok(new CreateOrderResponse(success.Order.ToPresentation())),

            _ => throw new UnreachableException(),
        };
    }

    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(GetAllResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetAllResponse>> SearchOrderHistoryItems(
        [FromQuery] int pageSize,
        CancellationToken cancellationToken,
        [FromQuery] long? lastSeenId = null)
    {
        if (pageSize < MinimalPageSize)
        {
            return BadRequest(new
            {
                message = $"Page size must be no less than {MinimalPageSize}"
            });
        }

        var serviceRequest = new GetAll.Request(
            pageSize,
            lastSeenId == null ? null : new PageToken(lastSeenId.Value));

        GetAll.Result serviceResult = await _orderDeliveryService.GetAllAsync(
            serviceRequest,
            cancellationToken);

        return serviceResult switch
        {
            GetAll.Result.Success success => Ok(new GetAllResponse(
                new List<Order>(
                    success.Orders.Select(x => x.ToPresentation())),
                success.PageToken?.LastSeenId)),

            _ => throw new UnreachableException(),
        };
    }
}