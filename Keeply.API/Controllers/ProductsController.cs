using System.Security.Claims;
using Keeply.Application.DTOs.Products;
using Keeply.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Keeply.API.Controllers;

[ApiController]
[Route("api/products")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetUserId();
        var products = await _productService.GetAllAsync(userId);
        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = GetUserId();
        var product = await _productService.GetByIdAsync(id, userId);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
    {
        var userId = GetUserId();
        var product = await _productService.CreateAsync(request, userId);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest request)
    {
        var userId = GetUserId();
        var product = await _productService.UpdateAsync(id, request, userId);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = GetUserId();
        var deleted = await _productService.DeleteAsync(id, userId);
        return deleted ? NoContent() : NotFound();
    }

    private Guid GetUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(ClaimTypes.Name)
            ?? User.FindFirstValue("sub");
        return Guid.Parse(value!);
    }
}
