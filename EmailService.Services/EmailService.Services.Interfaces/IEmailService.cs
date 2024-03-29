﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmailService.Business.Entities;
using System.ServiceModel;

namespace EmailService.Services.Interfaces
{
    [ServiceContract]
    public interface IEmailService
    {
        [OperationContract(IsOneWay = true)]
        void SendEmail(String pMessage, String pAddress);
    }
}
