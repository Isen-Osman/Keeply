using System.Security.Claims;
using Keeply.Application.DTOs.Claims;
using Keeply.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Keeply.API.Controllers;

[ApiController]
[Route("api/claims")]
[Authorize]
public class ClaimsController : ControllerBase
{
    private readonly WarrantyClaimService _claimService;

    public ClaimsController(WarrantyClaimService claimService)
    {
        _claimService = claimService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClaimRequest request)
    {
        var result = await _claimService.CreateAsync(GetUserId(), request);
        return result is null ? NotFound("Product not found.") : Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var claims = await _claimService.GetAllAsync(GetUserId());
        return Ok(claims);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var claim = await _claimService.GetByIdAsync(id, GetUserId());
        return claim is null ? NotFound() : Ok(claim);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateClaimStatusRequest request)
    {
        var claim = await _claimService.UpdateStatusAsync(id, GetUserId(), request);
        return claim is null ? BadRequest("Invalid claim or status.") : Ok(claim);
    }

    private Guid GetUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(ClaimTypes.Name)
            ?? User.FindFirstValue("sub");
        return Guid.Parse(value!);
    }
}
