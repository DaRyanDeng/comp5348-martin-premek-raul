using System;
using System.Messaging;
using System.ServiceModel;
using SystemWideLoggingClientNS;

namespace VideoMessageBus
{
    class Program
    {
        private const string PublishQueuePath = ".\\private$\\PublisherMessageQueueTransacted";
        public static readonly string AbandonMessagesQueuePath = ".\\private$\\AbandonMessagesQueue";
        public static readonly string PublisherServiceEndpointAddress = "net.msmq://localhost/private/PublisherMessageQueueTransacted";

        static void Main(string[] args)
        {



            SystemWideLogging.InitiateClass();


            SystemWideLogging.LogServiceClient.LogEvent("MessageBus :: VideoMessageBus\\Program.cs  :: static void Main(string[] args)", "Service started");
           
            
            EnsureMessageQueuesExists();

            using (var publisher = new ServiceHost(typeof (PublisherService)))
            using (var subscriber = new ServiceHost(typeof (SubscriptionService)))
            {
                try
                {
                    publisher.Open();
                    subscriber.Open();

                    Console.WriteLine("Message Bus Started Press Q to quit");
                    while (Console.ReadKey().Key != ConsoleKey.Q) ;
                }
                finally
                {
                    publisher.Close();
                    subscriber.Close();
                }
            }


            SystemWideLogging.LogServiceClient.LogEvent("MessageBus :: VideoMessageBus\\Program.cs  :: static void Main(string[] args)", "Service ended");

        }

        private static void EnsureMessageQueuesExists()
        {
            // Create the transacted MSMQ queue if necessary.
            if (!MessageQueue.Exists(PublishQueuePath))
                MessageQueue.Create(PublishQueuePath, true);

            if (!MessageQueue.Exists(AbandonMessagesQueuePath))
                MessageQueue.Create(AbandonMessagesQueuePath, true);
        }
    }
}
