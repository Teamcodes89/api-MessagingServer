using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api_MessagingServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectionsController : ControllerBase
    {

        [HttpGet("connect")]
        public async Task Connect()
        {
            try
            {
                if (HttpContext.WebSockets.IsWebSocketRequest)
                {
                    using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                    await ProcessConnectionRequest(webSocket);
                }
                else
                {
                    HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);

            }
        }
        private async Task ProcessConnectionRequest(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult receiveResult = null;
            while (true)
            {
                receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                string textReceived = Encoding.ASCII.GetString(buffer);
                if(receiveResult.CloseStatus.HasValue)
                {
                    break;
                }
                var connectionRequestObject = JsonConvert.DeserializeObject<Objects.External.ConnectionRequest>(textReceived);
                if (connectionRequestObject == null || String.IsNullOrEmpty(connectionRequestObject.PhoneNumber))
                {
                    await Handlers.Connection.Close(webSocket);
                    return;
                }
                Handlers.Connection.Request(webSocket, Handlers.Connection.GetClientIP(this), JsonConvert.DeserializeObject<Objects.External.ConnectionRequest>(textReceived));
            }
            await Handlers.Connection.Close(webSocket);
        }
    }
}
