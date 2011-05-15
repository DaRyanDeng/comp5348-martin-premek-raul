using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Messaging;

namespace EmailService.Process
{
    class Program
    {
        static void Main(string[] args)
        {
            EnsureMessageQueuesExists();
            HostServices();
        }

        private static readonly String sPublishQueuePath = ".\\private$\\EmailServiceQueue";
        private static void EnsureMessageQueuesExists()
        {
            // Create the transacted MSMQ queue if necessary.
            if (!MessageQueue.Exists(sPublishQueuePath))
                MessageQueue.Create(sPublishQueuePath, true);
        }

        private static void HostServices()
        {
            using (ServiceHost lHost = new ServiceHost(typeof(EmailService.Services.EmailService)))
            {
                lHost.Open();
                Console.WriteLine("Email service started. Press Q to quit.");
                while (Console.ReadKey().Key != ConsoleKey.Q) ;
            }
        }
    }
}
