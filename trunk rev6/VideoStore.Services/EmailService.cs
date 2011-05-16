using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoStore.Services.Interfaces;
using VideoStore.Business.Components.Interfaces;
using Microsoft.Practices.ServiceLocation;

namespace VideoStore.Services
{
    public class EmailService : IEmailService
    {
        public IEmailProvider EmailProvider
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IEmailProvider>();
            }
        }

        public void SendEmail(String message, String address)
        {
            EmailProvider.SendMessage(message, address);
        }
    }
}