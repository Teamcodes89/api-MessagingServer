using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_MessagingServer.Handlers
{
    public class ClientStatus
    {
        private static List<Objects.ClientStatus> clientStatuses = new List<Objects.ClientStatus>();
        public static void MarkClientAsOnline(Objects.ClientStatus client)
        {
            var clientStatus = clientStatuses.Where<Objects.ClientStatus>(x => x.PhoneNumber == client.PhoneNumber).First();
            if (clientStatus == null)
            {
                clientStatuses.Add(new Objects.ClientStatus()
                {
                    Connected = true,
                    PhoneNumber = client.PhoneNumber
                });
            }
            else
            {
                clientStatus.Connected = true;
            }
        }
        public static void MarkClientAsOffline(Objects.ClientStatus client)
        {
            var clientStatus = clientStatuses.Where<Objects.ClientStatus>(x => x.PhoneNumber == client.PhoneNumber).First();
            if (clientStatus == null)
            {
                clientStatuses.Add(new Objects.ClientStatus()
                {
                    Connected = false,
                    PhoneNumber = client.PhoneNumber
                });
            }
            else
            {
                clientStatus.Connected = false;
            }
        }
    }
}
