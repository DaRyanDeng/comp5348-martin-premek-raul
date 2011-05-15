using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoStore.Business.Components.Interfaces;
using VideoStore.Business.Components.EmailService;

namespace VideoStore.Business.Components
{
    public class EmailProvider : IEmailProvider
    {
        public void SendMessage(String pMessage, String address)
        {
            EmailServiceClient lClient = new EmailServiceClient();
            lClient.SendEmail(pMessage, address);
        }
    }
}