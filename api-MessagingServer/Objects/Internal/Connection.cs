using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace api_MessagingServer.Objects.Internal
{
    public class Connection
    {
        public WebSocket webSocket { get; set; }
        public string Number { get; set; }
        public string Ip { get; set; }
    }
}
