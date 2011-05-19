using System;
using System.Threading;
using SystemWideLoggingClientNS;


namespace DeliveryCo.Process
{
    class Program
    {
        static void Main(string[] args)
        {

            
            // Wait 3 seconds just in case if you launch this service by F5 on solution
            //Thread.Sleep(TimeSpan.FromSeconds(3)); << Premek: it may be more than 3 seconds, better if we run this project manually


            SystemWideLogging.InitiateClass();

            SystemWideLogging.LogServiceClient.LogEvent("DeliveryCo :: DeliveryCo.Application\\DeliveryCo.Process\\DeliveryLauncher.cs :: public void Run()", "Service started");


            using (var invoker = new DeliveryLauncher())
            {
                invoker.Run();
            }


            SystemWideLogging.LogServiceClient.LogEvent("DeliveryCo :: DeliveryCo.Application\\DeliveryCo.Process\\DeliveryLauncher.cs :: public void Run()", "Service ended");


        }
    }
}
