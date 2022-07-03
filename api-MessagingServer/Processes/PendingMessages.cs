using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace api_MessagingServer.Processes
{
    public class PendingMessages
    {
        private static Thread resendProcess = null;
        private static int interval = 1000;
        private static List<Objects.External.MessageRequest> messageRequests = new List<Objects.External.MessageRequest>();
        public static void Startup(IConfiguration configuration)
        {
            interval = configuration.GetValue<int>("Interval");
        }

        public static void AddToQueue(Objects.External.MessageRequest messageRequest)
        {
            messageRequests.Add(messageRequest);
        }

        public static List<Objects.External.MessageRequest> GetFromQueue(string receiverPhoneNumber)
        {
            return messageRequests.FindAll(x => x.Receiver.Equals(receiverPhoneNumber));
        }
        public static List<Objects.External.MessageRequest> GetFromQueue(Objects.Internal.Connection connection)
        {
            return messageRequests.FindAll(x => x.Receiver.Equals(connection.Number));
        }
        public static bool RemoveFromQueue(Objects.External.MessageRequest messageRequest)
        {
            return messageRequests.Remove(messageRequest);
        }
    }
}
