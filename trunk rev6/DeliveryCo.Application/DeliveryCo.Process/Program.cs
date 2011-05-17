using System;
using System.Threading;

namespace DeliveryCo.Process
{
    class Program
    {
        static void Main(string[] args)
        {
            // Wait 3 seconds just in case if you launch this service by F5 on solution
            Thread.Sleep(TimeSpan.FromSeconds(3));

            using (var invoker = new DeliveryLauncher())
            {
                invoker.Run();
            }
        }
    }
}
