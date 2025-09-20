using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TextsController : ControllerBase
{
    private static readonly Dictionary<string, string> Texts = new();

    [HttpPost("upload")]
    [AuthorizeApiKey]
    public IActionResult Upload([FromBody] string text, bool autoDelete = false)
    {
        var token = Guid.NewGuid().ToString("N");
        Texts[token] = text;
        return Ok(new { url = $"/api/texts/{token}?autoDelete={autoDelete}" });
    }

    [HttpGet("{token}")]
    [AllowAnonymous]
    public IActionResult Read(string token, bool autoDelete = false)
    {
        if (!Texts.ContainsKey(token)) return NotFound();

        var text = Texts[token];
        if (autoDelete) Texts.Remove(token);

        return Ok(new { text });
    }
}