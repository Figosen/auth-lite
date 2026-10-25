using Api.DTO;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthLogic authLogic) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(LoginRequest request)
    {
        try
        {
            var tokens = await authLogic.LoginAsync(request);
            return Ok(tokens);
        }
        catch (Exception e)
        {
            return Unauthorized($"Unauthorized: {e.Message}");
        }
    }
    [HttpPost("refresh")]
    public async Task<ActionResult<string>> refresh(RefreshRequest request)
    {
        try
        {
            var tokens = await authLogic.RefreshAsync(request);
            return Ok(tokens);
        }
        catch (Exception e)
        {
            return Unauthorized($"Unauthorized: {e.Message}");
        }
    }
    [HttpPost("register")]
    public async Task<ActionResult<string>> register(RegisterDto request)
    {
        try
        {
            var tokens = await authLogic.RegisterAsync(request);
            return Ok(tokens);
        }
        catch (Exception e)
        {
            return Unauthorized($"Unauthorized: {e.Message}");
        }
    }
}
