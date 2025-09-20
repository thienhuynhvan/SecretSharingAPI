namespace SafeChat.Models;

public class FileUploadDto
{
    public IFormFile File { get; set; } = default!;
    public bool AutoDelete { get; set; }
}
