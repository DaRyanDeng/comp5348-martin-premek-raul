using System;
using Common;
using EmailService.Services.Interfaces;

namespace EmailService.Services
{
    public class EmailService : IEmailService
    {
        public void SendEmail(String pMessage, String pAddress)
        {
            ConsoleHelper.WriteLine(ConsoleColor.Green, "Send to: " + pAddress);
            Console.WriteLine(pMessage);
        }
    }
}
