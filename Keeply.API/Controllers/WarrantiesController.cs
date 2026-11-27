using Keeply.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Keeply.API.Extensions;

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
        var warranties = await _warrantyService.GetAllAsync(User.GetUserId());
        return Ok(warranties);
    }

    [HttpGet("expiring")]
    public async Task<IActionResult> GetExpiring()
    {
        var warranties = await _warrantyService.GetExpiringAsync(User.GetUserId());
        return Ok(warranties);
    }

    [HttpGet("expired")]
    public async Task<IActionResult> GetExpired()
    {
        var warranties = await _warrantyService.GetExpiredAsync(User.GetUserId());
        return Ok(warranties);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var warranty = await _warrantyService.GetByIdAsync(id, User.GetUserId());
        return warranty is null ? NotFound() : Ok(warranty);
    }

}
