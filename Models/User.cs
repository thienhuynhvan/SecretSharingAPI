namespace SafeChat.Models;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}

public class UserRegisterDto
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}

public class UserLoginDto
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}
