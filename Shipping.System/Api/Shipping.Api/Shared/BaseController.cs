using Microsoft.AspNetCore.Mvc;

namespace Shipping.Api.Shared;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    protected string GetUserId()
    {
        var ss = HttpContext.User.FindFirst("UserId")?.Value ?? "0";
        return ss;
    }
}