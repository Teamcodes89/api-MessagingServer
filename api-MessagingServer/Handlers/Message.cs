using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace api_MessagingServer.Handlers
{
    public class Message
    {
        public static async Task Request(Objects.Internal.Connection sender,Objects.External.MessageRequest messageRequest)
        {
            var receiverWebSocket = Handlers.Connection.GetByPhone(messageRequest.Receiver);
            if(receiverWebSocket != null && receiverWebSocket.webSocket.State == WebSocketState.Open)
            {
                await Send(sender, receiverWebSocket, messageRequest);
            }
            else
            {
                Processes.PendingMessages.AddToQueue(messageRequest);
            }
        }


        public static async Task Send(Objects.Internal.Connection sender, Objects.Internal.Connection receiver, Objects.External.MessageRequest messageRequest)
        {
            if(receiver.webSocket.State == WebSocketState.Open)
            {
                await receiver.webSocket.SendAsync(new ArraySegment<byte>(messageRequest.Text, 0, messageRequest.Text.Count()),
                WebSocketMessageType.Text,
                false,
                CancellationToken.None);
            }
        }

        public static bool IsTextValid(byte[] textArray)
        {
            if(textArray.Length > 0)
            {
                int emptySpaces = 0;
                foreach(var character in textArray)
                {
                    if(character == ' ')
                    {
                        emptySpaces++;
                    }
                }
                return emptySpaces == textArray.Count() ? false : true;
            }
            return false;
        }
    }
}
