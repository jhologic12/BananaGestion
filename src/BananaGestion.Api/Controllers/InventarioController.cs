using BananaGestion.Application.Modules.Inventario.Commands;
using BananaGestion.Application.Modules.Inventario.DTOs;
using BananaGestion.Application.Modules.Inventario.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BananaGestion.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InventarioController : ControllerBase
{
    private readonly IMediator _mediator;

    public InventarioController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("products")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
    {
        var products = await _mediator.Send(new GetProductsQuery());
        return Ok(products);
    }

    [HttpGet("products/low-stock")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetLowStock()
    {
        var products = await _mediator.Send(new GetLowStockProductsQuery());
        return Ok(products);
    }

    [HttpGet("products/{id}")]
    public async Task<ActionResult<ProductDto>> GetProductById(Guid id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id));
        return Ok(product);
    }

    [Authorize(Roles = "Administrador,Supervisor")]
    [HttpPost("products")]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductRequest request)
    {
        var product = await _mediator.Send(new CreateProductCommand(request));
        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
    }

    [Authorize(Roles = "Administrador,Supervisor")]
    [HttpPut("products/{id}")]
    public async Task<ActionResult<ProductDto>> UpdateProduct(Guid id, [FromBody] UpdateProductRequest request)
    {
        var product = await _mediator.Send(new UpdateProductCommand(id, request));
        return Ok(product);
    }

    [Authorize(Roles = "Administrador")]
    [HttpDelete("products/{id}")]
    public async Task<ActionResult<bool>> DeleteProduct(Guid id)
    {
        var result = await _mediator.Send(new DeleteProductCommand(id));
        return Ok(result);
    }

    [HttpGet("movements")]
    public async Task<ActionResult<IEnumerable<InventoryMovementDto>>> GetMovements(
        [FromQuery] Guid? productId = null,
        [FromQuery] DateTime? start = null,
        [FromQuery] DateTime? end = null)
    {
        var movements = await _mediator.Send(new GetInventoryMovementsQuery(productId, start, end));
        return Ok(movements);
    }

    [Authorize(Roles = "Administrador,Supervisor")]
    [HttpPost("movements")]
    public async Task<ActionResult<InventoryMovementDto>> CreateMovement(
        [FromBody] CreateInventoryMovementRequest request)
    {
        var userId = Guid.Parse(User.FindFirst("sub")!.Value);
        var movement = await _mediator.Send(new CreateInventoryMovementCommand(request, userId));
        return Created("", movement);
    }
}
