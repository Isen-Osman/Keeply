using System.Security.Claims;
using Keeply.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Keeply.API.Controllers;

[ApiController]
[Route("api/warranties")]
[Authorize]
public class WarrantiesController : ControllerBase
{
    private readonly WarrantyService _warrantyService;

    public WarrantiesController(WarrantyService warrantyService)
    {
        _warrantyService = warrantyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var warranties = await _warrantyService.GetAllAsync(GetUserId());
        return Ok(warranties);
    }

    [HttpGet("expiring")]
    public async Task<IActionResult> GetExpiring()
    {
        var warranties = await _warrantyService.GetExpiringAsync(GetUserId());
        return Ok(warranties);
    }

    [HttpGet("expired")]
    public async Task<IActionResult> GetExpired()
    {
        var warranties = await _warrantyService.GetExpiredAsync(GetUserId());
        return Ok(warranties);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var warranty = await _warrantyService.GetByIdAsync(id, GetUserId());
        return warranty is null ? NotFound() : Ok(warranty);
    }

    private Guid GetUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(ClaimTypes.Name)
            ?? User.FindFirstValue("sub");
        return Guid.Parse(value!);
    }
}
