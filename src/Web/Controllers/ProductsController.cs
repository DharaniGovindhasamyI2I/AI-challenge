using MediatR;
using Microsoft.AspNetCore.Mvc;
using CleanArchitecture.Application.Products.Commands.CreateProduct;
using CleanArchitecture.Application.Products.Commands.UpdateProduct;
using CleanArchitecture.Application.Products.Commands.DeleteProduct;
using CleanArchitecture.Application.Products.Queries.GetProductById;
using CleanArchitecture.Application.Products.Queries.GetProducts;
using CleanArchitecture.Application.Products.Commands.UpdateInventory;
using CleanArchitecture.Application.Products.Common;

namespace CleanArchitecture.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/products?name=foo&categoryId=1&minPrice=10&maxPrice=100
    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> Get([FromQuery] GetProductsQuery query)
    {
        var products = await _mediator.Send(query);
        return Ok(products);
    }

    // GET: api/products/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id));
        if (product == null)
            return NotFound();
        return Ok(product);
    }

    // POST: api/products
    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductCommand command)
    {
        var product = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    // PUT: api/products/{id}/inventory
    [HttpPut("{id}/inventory")]
    public async Task<ActionResult<ProductDto>> UpdateInventory(int id, [FromBody] UpdateInventoryCommand command)
    {
        if (id != command.ProductId)
            return BadRequest("Product ID mismatch.");
        var product = await _mediator.Send(command);
        return Ok(product);
    }

    // PUT: api/products/{id}
    [HttpPut("{id}")]
    public IActionResult Update(int id)
    {
        // Implement UpdateProductCommand and handler if needed
        return StatusCode(501, "Update product not implemented yet.");
    }

    // DELETE: api/products/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        // Implement DeleteProductCommand and handler if needed
        return StatusCode(501, "Delete product not implemented yet.");
    }
} 