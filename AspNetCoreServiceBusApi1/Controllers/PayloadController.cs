using AspNetCoreServiceBusApi1.Model;
using Microsoft.AspNetCore.Mvc;
using ServiceBusMessaging;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreServiceBusApi1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PayloadController : Controller
{
    private readonly ServiceBusSender _serviceBusSender;

    public PayloadController(ServiceBusSender serviceBusSender)
    {
        _serviceBusSender = serviceBusSender;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<List<Payload>> Get()
    {
        return Ok(data);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Payload> Get(int id)
    {
        if (id == 0)
        {
            return BadRequest();
        }

        var result = data.Where(d => d.Id == id);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Payload), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Payload), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody][Required] Payload request)
    {
        if (data.Any(d => d.Id == request.Id))
        {
            return Conflict($"data with id {request.Id} already exists");
        }

        data.Add(request);

        // Send this to the bus for the other services
        await _serviceBusSender.SendMessage(new MyPayload
        {
            Goals = request.Goals,
            Name = request.Name,
            Delete = false
        });

        return Ok(request);
    }

    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(typeof(Payload), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Update(int id, [FromBody][Required] Payload request)
    {
        if (!data.Any(d => d.Id == request.Id))
        {
            return NotFound($"data with id {id} does not exist");
        }

        var item = data.First(d => d.Id == id);
        item.Name = request.Name;
        item.Goals = request.Goals;

        // Send this to the bus for the other services
        await _serviceBusSender.SendMessage(new MyPayload
        {
            Goals = request.Goals,
            Name = request.Name,
            Delete = false
        });

        return Ok(request);
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (id == 0)
        {
            return BadRequest();
        }

        if (!data.Any(d => d.Id == id))
        {
            return NotFound($"data with id {id} does not exist");
        }

        var item = data.First(d => d.Id == id);
        data.Remove(item);

        // Send this to the bus for the other services
        await _serviceBusSender.SendMessage(new MyPayload
        {
            Goals = item.Goals,
            Name = item.Name,
            Delete = true
        });

        return Ok();
    }

    private static readonly List<Payload> data = new()
    {
        new Payload{ Id=1, Goals=3, Name="wow"},
        new Payload{ Id=2, Goals=4, Name="not so bad"},
    };
}
