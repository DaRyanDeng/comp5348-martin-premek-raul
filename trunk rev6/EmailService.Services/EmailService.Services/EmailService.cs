using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmailService.Services.Interfaces;
using EmailService.Business.Entities;

namespace EmailService.Services
{
    public class EmailService : IEmailService
    {
        public void SendEmail(String pMessage, String pAddress)
        {
            Console.WriteLine("Send to: " + pAddress);
            Console.WriteLine(pMessage);
        }
    }
}
