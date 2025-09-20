using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeChat.Models;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private static readonly Dictionary<string, string> Files = new();

    [HttpPost("upload")]
    [AuthorizeApiKey]
    public async Task<IActionResult> Upload([FromForm] FileUploadDto dto)
    {
        if (dto.File == null || dto.File.Length == 0) 
            return BadRequest("Empty file");

        var folder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
        Directory.CreateDirectory(folder);

        var token = Guid.NewGuid().ToString("N");
        var filePath = Path.Combine(folder, token + Path.GetExtension(dto.File.FileName));

        using var stream = new FileStream(filePath, FileMode.Create);
        await dto.File.CopyToAsync(stream);

        Files[token] = filePath;

        return Ok(new { url = $"/api/files/{token}?autoDelete={dto.AutoDelete}" });
    }


    [HttpGet("{token}")]
    [AllowAnonymous] // ai cũng truy cập được
    public IActionResult Download(string token, bool autoDelete = false)
    {
        if (!Files.ContainsKey(token)) return NotFound();

        var filePath = Files[token];
        var content = System.IO.File.ReadAllBytes(filePath);
        var fileName = Path.GetFileName(filePath);

        if (autoDelete)
        {
            System.IO.File.Delete(filePath);
            Files.Remove(token);
        }

        return File(content, "application/octet-stream", fileName);
    }
}