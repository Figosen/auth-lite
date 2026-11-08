using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[Authorize]
[ApiController]
[Route("[controller]")]
public class ExampleController() : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<string>> Get()
    {
        return Ok(User.FindFirstValue(ClaimTypes.NameIdentifier)); //TODO: virker nok ikke når jeg får lavet middlewaren selv
    }
}
