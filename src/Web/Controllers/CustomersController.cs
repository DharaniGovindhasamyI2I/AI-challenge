using MediatR;
using Microsoft.AspNetCore.Mvc;
using CleanArchitecture.Application.Customers.Commands.CreateCustomer;
using CleanArchitecture.Application.Customers.Commands.UpdateCustomer;
using CleanArchitecture.Application.Customers.Commands.DeleteCustomer;
using CleanArchitecture.Application.Customers.Queries.GetCustomerById;
using CleanArchitecture.Application.Customers.Queries.GetCustomers;

namespace CleanArchitecture.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetCustomersQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetCustomerByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCustomerCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCustomerCommand command)
    {
        if (id != command.Id) return BadRequest();
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteCustomerCommand(id));
        return NoContent();
    }
} 