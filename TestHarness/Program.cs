using System;
using System.Text;
using CannedLogic.RabbitMqMessaging;
using Newtonsoft.Json;

namespace TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            IMessageSender ms = new RabbitMqMessageSender("TestQ");
            Console.WriteLine("Connected to TestQ...");

            var o = new Wibbler("Hello Matey");
            ms.PublishObjectAsMessage(o);

            RabbitMqMessageReceiver mr = new RabbitMqMessageReceiver("TestQ");
            mr.Consumer.Received += (sender, e) =>
            {
                var body = e.Body;
                var message = Encoding.UTF8.GetString(body);
                Wibbler w = JsonConvert.DeserializeObject<Wibbler>(message);
                Console.SetCursorPosition(0, Console.CursorTop - 2);
                Console.WriteLine($" [x] Received {w.Id} on {mr.QueueName}, containing the comment {w.Comment}");
                Console.WriteLine("                                                                           ");
                Console.WriteLine("Press [enter] to exit...");

            };

            mr.ConsumeMessage();
            Console.WriteLine("Press [enter] to exit...");

            var t = new System.Timers.Timer(5000);
            t.Elapsed += (sender, e) =>
            {
                ms.PublishObjectAsMessage(o);
            };

            t.Start();

            Console.ReadLine();
 
            ms.Dispose();
            mr.Dispose();
        }
    }

 
}
