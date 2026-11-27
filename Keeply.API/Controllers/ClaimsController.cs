using Keeply.API.Extensions;
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
        var result = await _claimService.CreateAsync(User.GetUserId(), request);
        return result is null ? Problem(title: "Product not found.", statusCode: StatusCodes.Status404NotFound) : Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var claims = await _claimService.GetAllAsync(User.GetUserId());
        return Ok(claims);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var claim = await _claimService.GetByIdAsync(id, User.GetUserId());
        return claim is null ? NotFound() : Ok(claim);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateClaimStatusRequest request)
    {
        var (success, error, claim) = await _claimService.UpdateStatusAsync(id, User.GetUserId(), request);
        if (success) return Ok(claim);
        return error == "Claim not found." ? NotFound(error) : BadRequest(error);
    }
}
