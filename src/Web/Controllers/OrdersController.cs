using MediatR;
using Microsoft.AspNetCore.Mvc;
using CleanArchitecture.Application.Orders.Commands.CreateOrder;
using CleanArchitecture.Application.Orders.Commands.UpdateOrderStatus;
using CleanArchitecture.Application.Orders.Queries.GetOrderById;
using CleanArchitecture.Application.Orders.Queries.GetOrders;
using CleanArchitecture.Application.Orders.Common;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Application.Common.Models;

namespace CleanArchitecture.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/orders?customerId=1&status=Pending&pageNumber=1&pageSize=10
    [HttpGet]
    public async Task<ActionResult<PaginatedList<OrderDto>>> Get([FromQuery] GetOrdersQuery query)
    {
        var orders = await _mediator.Send(query);
        return Ok(orders);
    }

    // GET: api/orders/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetById(int id)
    {
        var order = await _mediator.Send(new GetOrderByIdQuery(id));
        if (order == null)
            return NotFound();
        return Ok(order);
    }

    // POST: api/orders
    [HttpPost]
    public async Task<ActionResult<OrderDto>> Create([FromBody] CreateOrderCommand command)
    {
        var order = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    // PUT: api/orders/{id}/status
    [HttpPut("{id}/status")]
    public async Task<ActionResult<OrderDto>> UpdateStatus(int id, [FromBody] UpdateOrderStatusCommand command)
    {
        if (id != command.OrderId)
            return BadRequest("Order ID mismatch.");
        
        var order = await _mediator.Send(command);
        return Ok(order);
    }

    // PUT: api/orders/{id}/confirm
    [HttpPut("{id}/confirm")]
    public async Task<ActionResult<OrderDto>> Confirm(int id)
    {
        var command = new UpdateOrderStatusCommand { OrderId = id, NewStatus = OrderStatus.Confirmed };
        var order = await _mediator.Send(command);
        return Ok(order);
    }

    // PUT: api/orders/{id}/ship
    [HttpPut("{id}/ship")]
    public async Task<ActionResult<OrderDto>> Ship(int id)
    {
        var command = new UpdateOrderStatusCommand { OrderId = id, NewStatus = OrderStatus.Shipped };
        var order = await _mediator.Send(command);
        return Ok(order);
    }

    // PUT: api/orders/{id}/deliver
    [HttpPut("{id}/deliver")]
    public async Task<ActionResult<OrderDto>> Deliver(int id)
    {
        var command = new UpdateOrderStatusCommand { OrderId = id, NewStatus = OrderStatus.Delivered };
        var order = await _mediator.Send(command);
        return Ok(order);
    }

    // PUT: api/orders/{id}/cancel
    [HttpPut("{id}/cancel")]
    public async Task<ActionResult<OrderDto>> Cancel(int id)
    {
        var command = new UpdateOrderStatusCommand { OrderId = id, NewStatus = OrderStatus.Cancelled };
        var order = await _mediator.Send(command);
        return Ok(order);
    }
} 