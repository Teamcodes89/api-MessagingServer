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
        public static void Startup(IConfiguration configuration)
        {
            interval = configuration.GetValue<int>("Interval");
            resendProcess = new Thread(ReSend);
            resendProcess.Start();
        }

        private static void ReSend()
        {
            while(true)
            {
                Thread.Sleep(interval);
            }
        }

        public static void AddToQueue(Objects.External.MessageRequest messageRequest)
        {

        }
    }
}
