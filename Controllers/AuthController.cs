using Microsoft.AspNetCore.Mvc;
using SafeChat.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private static readonly List<User> Users = new();

    [HttpPost("register")]
    public IActionResult Register(UserRegisterDto dto)
    {
        if (Users.Any(u => u.Email == dto.Email))
            return BadRequest("Email already exists");

        var user = new User { Id = Guid.NewGuid(), Email = dto.Email, Password = dto.Password };
        Users.Add(user);
        return Ok("User registered");
    }

    [HttpPost("login")]
    public IActionResult Login(UserLoginDto dto)
    {
        var user = Users.FirstOrDefault(u => u.Email == dto.Email && u.Password == dto.Password);
        if (user == null) return Unauthorized();

        var key = ApiKeyStore.GenerateKey(user.Email);
        return Ok(new { apiKey = key });
    }
}