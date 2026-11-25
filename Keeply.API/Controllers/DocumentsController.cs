using System.Security.Claims;
using Keeply.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Keeply.API.Controllers;

[ApiController]
[Route("api/documents")]
[Authorize]
public class DocumentsController : ControllerBase
{
    private readonly DocumentService _documentService;

    public DocumentsController(DocumentService documentService)
    {
        _documentService = documentService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] Guid productId, [FromForm] IFormFile file)
    {
        if (file.Length == 0)
            return BadRequest("File is required.");

        await using var stream = file.OpenReadStream();
        var result = await _documentService.UploadAsync(GetUserId(), productId, file.FileName, stream);
        return result is null ? NotFound("Product not found.") : Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var (document, filePath) = await _documentService.GetByIdAsync(id, GetUserId());
        if (document is null || filePath is null || !System.IO.File.Exists(filePath))
            return NotFound();

        var contentType = "application/octet-stream";
        return PhysicalFile(filePath, contentType, document.FileName);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _documentService.DeleteAsync(id, GetUserId());
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
