using System.Collections.Generic;
using System.Linq;
using AspNetCoreServiceBusApi2.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreServiceBusApi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewPayloadMessagesController : Controller
    {

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<Payload>> Get()
        {
            return Ok(DataServiceSimi.Data);
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

            var result = DataServiceSimi.Data.Where(d => d.Name == name);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

    }
}
