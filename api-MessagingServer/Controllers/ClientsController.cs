using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_MessagingServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        [HttpPost("status/online")]
        public IActionResult MarkStatusOnLine([FromBody] Objects.ClientStatus clientStatus)
        {
            
            try
            {

                Handlers.ClientStatus.MarkClientAsOnline(clientStatus);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return Ok();
        }
        [HttpPost("status/offline")]
        public IActionResult MarkStatusOffLine([FromBody] Objects.ClientStatus clientStatus)
        {
            try
            {

                Handlers.ClientStatus.MarkClientAsOffline(clientStatus);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return Ok();
        }
    }
}
