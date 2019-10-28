using System;
using System.Text;
using CannedLogic.RabbitMqMessaging;

namespace TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            IMessageSender ms = new RabbitMqMessageSender("TestQ");
            Console.WriteLine("Connected to TestQ...");
            ms.PublishMessage("Well Hello There!");
            Console.WriteLine("Message Sent! Now let's see if we can get it back...");

            RabbitMqMessageReceiver mr = new RabbitMqMessageReceiver("TestQ");

            mr.Consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] Received {message} on {mr.QueueName}");
            };

            mr.ConsumeMessage();

            Console.WriteLine("Now let's try a second one...");

            ms.PublishMessage("Can you hear me over there?");
 
            ms.Dispose();
            mr.Dispose();
        }
    }
}
