using Keeply.API.Contracts.Documents;
using Keeply.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Keeply.API.Extensions;

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
    public async Task<IActionResult> Upload([FromForm] UploadDocumentRequest request)
    {
        try
        {
            await using var stream = request.File.OpenReadStream();
            var result = await _documentService.UploadAsync(User.GetUserId(), request.ProductId, request.File.FileName, stream, request.File.Length);
            return result is null ? Problem(title: "Product not found.", statusCode: StatusCodes.Status404NotFound) : Ok(result);
        }
        catch (ArgumentException ex)
        {
            return Problem(title: ex.Message, statusCode: StatusCodes.Status400BadRequest);
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var (document, filePath) = await _documentService.GetByIdAsync(id, User.GetUserId());
        if (document is null || filePath is null || !System.IO.File.Exists(filePath))
            return NotFound();

        var contentType = "application/octet-stream";
        return PhysicalFile(filePath, contentType, document.FileName);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _documentService.DeleteAsync(id, User.GetUserId());
        return deleted ? NoContent() : NotFound();
    }

}
