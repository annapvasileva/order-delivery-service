using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderDeliveryService.Application.Contracts.Orders;
using OrderDeliveryService.Presentation.Models.Operations.Requests;
using OrderDeliveryService.Presentation.Models.Operations.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderDeliveryService.Presentation.Controllers;

[ApiController]
[Route("/v1/orders")]
public class OrderController : ControllerBase
{
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
        
    }
}