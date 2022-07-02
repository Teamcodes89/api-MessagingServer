using api_MessagingServer.Objects;
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

namespace api_MessagingServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        [HttpPost("send")]
        public async Task Send([FromBody] Objects.External.MessageRequest message)
        {
            try
            {
                if (HttpContext.WebSockets.IsWebSocketRequest)
                {
                    using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                    await ProcessMessageRequest(webSocket);
                }
                else
                {
                    HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        /*
        [HttpGet("get")]
        public List<Message> Get([FromBody] Objects.Internal.Connection connection)
        {
            return Handlers.Message.GetMessages(connection);
        }
        */
        private async Task ProcessMessageRequest(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult receiveResult = null;
            while (true)
            {
                receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                string textReceived = Encoding.ASCII.GetString(buffer);
                if (receiveResult.CloseStatus.HasValue)
                {
                    break;
                }
                var messageRequest = JsonConvert.DeserializeObject<Objects.External.MessageRequest>(textReceived);
                if (messageRequest == null)
                {
                    break;
                }
                if (String.IsNullOrEmpty(messageRequest.Date))
                {
                    break;
                }
                if (String.IsNullOrEmpty(messageRequest.Receiver))
                {
                    break;
                }
                if (String.IsNullOrEmpty(messageRequest.Sender))
                {
                    break;
                }
                if (Handlers.Message.IsTextValid(messageRequest.Text) == false)
                {
                    break;
                }
                if (String.IsNullOrEmpty(messageRequest.Time))
                {
                    break;
                }

                var senderConnectionObject = new Objects.Internal.Connection()
                {
                    webSocket = webSocket,
                    Ip = Handlers.Connection.GetClientIP(this),
                    Number = messageRequest.Sender
                };

                await Handlers.Message.Request(senderConnectionObject, JsonConvert.DeserializeObject<Objects.External.MessageRequest>(textReceived));
            }
            await Handlers.Connection.Close(webSocket);
        }
    }
}
