using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace CannedLogic.RabbitMqMessaging
{
    public class RabbitMqMessageSender : IMessageSender
    {
        public string HostName { get; } = "localhost";
        public string ExchangeName { get; } = "";
        public string QueueName { get; }
        private readonly IConnectionFactory factory;
        private readonly IConnection connection;
        private readonly IModel channel;
        bool disposed = false;

        public RabbitMqMessageSender(string hostName, string exchangeName, string queueName)
        {
            this.HostName = hostName;
            this.ExchangeName = exchangeName;
            this.QueueName = queueName;
            this.factory = new ConnectionFactory();
            this.connection = factory.CreateConnection();
            this.channel = connection.CreateModel();
            channel.QueueDeclare(queue: QueueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null); 
            
        }

        public RabbitMqMessageSender(string exchangeName, string queueName)
        {
            this.ExchangeName = exchangeName;
            this.QueueName = queueName;
            this.factory = new ConnectionFactory();
            this.connection = factory.CreateConnection();
            this.channel = connection.CreateModel();
            channel.QueueDeclare(queue: QueueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

        }

        public RabbitMqMessageSender(string queueName)
        {
            this.QueueName = queueName;
            this.factory = new ConnectionFactory();
            this.connection = factory.CreateConnection();
            this.channel = connection.CreateModel();
            ConnectToQueue();
        }

        private void ConnectToQueue()
        {
            Console.WriteLine($"Connecting to {QueueName}");
            channel.QueueDeclare(queue: QueueName,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
        }

        public void PublishMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(ExchangeName, QueueName, null, body);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                channel.Dispose();
                connection.Dispose();
            }

            disposed = true;
        }

        public void PublishObjectAsMessage(object o)
        {
            string message = JsonConvert.SerializeObject(o);
            PublishMessage(message);
        }

        ~RabbitMqMessageSender()
        {
            Dispose(false);
        }
    }
}