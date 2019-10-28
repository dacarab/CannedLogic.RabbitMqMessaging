using System;
using System.Collections.Generic;
using System.Text;

namespace CannedLogic.RabbitMqMessaging
{
    public interface IMessageReceiver : IDisposable
    {
        public void ConsumeMessage();
    }
}