using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class SecureController : ControllerBase
{
    [HttpGet]
    [Authorize] // Requires JWT authentication
    public IActionResult GetSecureData()
    {
        // This code will only execute if the user is authenticated with a valid JWT token
        return Ok("This is secure data accessible only to authenticated users.");
    }
}