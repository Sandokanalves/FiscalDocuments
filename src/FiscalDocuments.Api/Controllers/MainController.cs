using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FiscalDocuments.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class MainController : ControllerBase
{
    protected IActionResult HandleResult<T>(T result)
    {
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}

