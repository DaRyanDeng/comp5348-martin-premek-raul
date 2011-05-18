using System;
using Common;
using EmailService.Services.Interfaces;
using System.ServiceModel;
using SystemWideLoggingClientNS;

namespace EmailService.Services
{
    public class EmailService : IEmailService
    {
        public void SendEmail(String pMessage, String pAddress)
        {
            SystemWideLogging.LogServiceClient.LogEvent("EmailService :: EmailService.Services\\EmailService.Services\\EmailService.cs   :: public void SendEmail(String pMessage, String pAddress)", "Received an MSMQ Request and Sending Email ( pMessage=" + pMessage + " , pAddress " + pAddress + " )");
            ConsoleHelper.WriteLine(ConsoleColor.Green, "Send to: " + pAddress);
            Console.WriteLine(pMessage);
        }
    }
}
