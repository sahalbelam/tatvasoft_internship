using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtTokenService _jwtTokenService;

    public AuthController(JwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel login)
    {
        // Replace with your own user validation logic
        if (login.Username == "admin" && login.Password == "password")
        {
            var token = _jwtTokenService.GenerateToken(login.Username);
            return Ok(new { token });
        }

        return Unauthorized();
    }
}

public class LoginModel
{
  
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}