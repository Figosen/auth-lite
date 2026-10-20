using System;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public ActionResult<string> Login(LoginRequest request)
    {
        var rand = new Random();
        if (rand.NextInt64(1,4) == 2)
        {
            return Unauthorized("Øv");
        }
        return Ok("test");
    }
    [HttpPost("refresh")]
    public ActionResult<string> refresh(RefreshRequest request)
    {
        return Ok(request.RefreshToken); // Noice
    }
}
