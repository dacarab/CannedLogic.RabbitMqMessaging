using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CannedLogic.RabbitMqMessaging
{
    public class RabbitMqMessageReceiver : IMessageReceiver
    {
        public string HostName { get; } = "localhost";
        public string ExchangeName { get; } = "";
        public string QueueName { get; }
        private readonly IConnectionFactory factory;
        private readonly IConnection connection;
        private readonly IModel channel;
        public EventingBasicConsumer Consumer { get;  }
        private bool disposed;
        private void ConnectToQueue()
        {
            Console.WriteLine($"Connecting to {QueueName}");
            channel.QueueDeclare(queue: QueueName,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
        }

        public RabbitMqMessageReceiver(string hostName, string exchangeName, string queueName)
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

        public RabbitMqMessageReceiver(string exchangeName, string queueName)
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

        public RabbitMqMessageReceiver(string queueName)
        {
            this.QueueName = queueName;
            this.factory = new ConnectionFactory();
            this.connection = factory.CreateConnection();
            this.channel = connection.CreateModel();
            ConnectToQueue();
            this.Consumer = new EventingBasicConsumer(channel);
        }

        public void ConsumeMessage()
        {
            var handler = typeof(EventingBasicConsumer)
                          .GetField("Received",System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                          .GetValue(Consumer) as Delegate;

            if (handler == null) throw new InvalidOperationException(); // TODO: Replace with a Custom exception that explains the issue.
            else
            {
                channel.BasicConsume(queue: QueueName,
                     autoAck: true,
                     consumer: Consumer);
            }

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

        ~RabbitMqMessageReceiver()
        {
            Dispose(false);
        }
    }
}