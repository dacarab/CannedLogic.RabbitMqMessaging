using System;
using System.Collections.Generic;
using System.Text;

namespace CannedLogic.RabbitMqMessaging
{
    public interface IMessageSender : IDisposable
    {
        public void PublishMessage(string message);
        public void PublishObjectAsMessage(object o);
    }
}