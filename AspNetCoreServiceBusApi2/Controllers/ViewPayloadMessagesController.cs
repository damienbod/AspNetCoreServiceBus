using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreServiceBusApi2.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreServiceBusApi2.Controllers
{
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
        public async Task<ActionResult<List<Payload>>> Get()
        {
            return Ok(await _context.Payloads.ToListAsync());
        }

        [HttpGet("{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Payload>> Get(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest();
            }

            var result = await _context.Payloads.FirstorDefaultAsync(d => d.Name == name);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
