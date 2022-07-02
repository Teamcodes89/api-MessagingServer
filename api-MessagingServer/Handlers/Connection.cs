
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace api_MessagingServer.Handlers
{
    public static class Connection
    {
        public static List<Objects.Internal.Connection> connectionList = new List<Objects.Internal.Connection>();


        public static void Request(WebSocket webSocket, string ip, Objects.External.ConnectionRequest connectionRequest)
        {
            var connectionObject = new Objects.Internal.Connection()
            {
                webSocket = webSocket,
                Ip = ip,
                Number = connectionRequest.PhoneNumber
            };
            _ = Handlers.Connection.Add(connectionObject) == false ? Handlers.Connection.Update(connectionObject) : false;

        }

        public static bool Update(Objects.Internal.Connection connectionObject)
        {
            if(Contains(connectionObject))
            {
                var connectionElement = connectionList.Where(x => x.Ip == connectionObject.Ip && x.Number == connectionObject.Number).First();
                connectionElement.Number = connectionObject.Number;
                connectionElement.webSocket = connectionObject.webSocket;
                connectionElement.Ip = connectionObject.Ip;
                return true;
            }
            return false;
        }

        public static bool Add(Objects.Internal.Connection connectionObject)
        {
            if (!Contains(connectionObject))
            {
                connectionList.Add(new Objects.Internal.Connection()
                {
                    Number = connectionObject.Number,
                    webSocket = connectionObject.webSocket,
                    Ip = connectionObject.Ip
                });
                return true;
            }
            return false;
        }

        public static Objects.Internal.Connection GetByIp(string ip)
        {
            return connectionList.Where(x => x.Ip == ip).FirstOrDefault();
        }

        public static IEnumerable<Objects.Internal.Connection> GetAllByIp(string ip)
        {
            return connectionList.Where(x => x.Ip == ip);
        }

        public static Objects.Internal.Connection GetByPhone(string phone)
        {
            return connectionList.Where(x => x.Number == phone).FirstOrDefault();
        }

        public static IEnumerable<Objects.Internal.Connection> GetAllByPhone(string phone)
        {
            return connectionList.Where(x => x.Number == phone );
        }

        public static bool Contains(Objects.Internal.Connection connectionObject)
        {
            return connectionList.Where(x => x.Ip == connectionObject.Ip && x.Number == connectionObject.Number && x.webSocket == connectionObject.webSocket).FirstOrDefault() == null ? false : true;
        }

        public static int Remove(WebSocket webSocket)
        {

            return connectionList.RemoveAll(x => x.webSocket == webSocket);
        }

        public static int Remove(string phoneNumber)
        {
            return connectionList.RemoveAll(x => x.Number == phoneNumber);
        }

        public static async Task Close(WebSocket webSocket)
        {

            Handlers.Connection.Remove(webSocket);
            await webSocket.CloseAsync(
                WebSocketCloseStatus.NormalClosure,
                "Closed by the server",
                CancellationToken.None);
        }

        public static string GetClientIP(this Microsoft.AspNetCore.Mvc.ControllerBase httpContext)
        {
            return httpContext.Request.HttpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}
