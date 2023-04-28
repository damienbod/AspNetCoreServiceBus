using AspNetCoreServiceBusApi2.Model;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreServiceBusApi2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ViewPayloadMessagesController : Controller
{
    private readonly PayloadContext _context;

    public ViewPayloadMessagesController(PayloadContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<List<Payload>> Get()
    {
        return Ok(_context.Payloads.ToList());
    }

    [HttpGet("{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Payload> Get(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest();
        }

        var result = _context.Payloads.FirstOrDefault(d => d.Name == name);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}
