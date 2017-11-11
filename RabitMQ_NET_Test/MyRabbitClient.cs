using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;

namespace RabitMQ_NET_Test
{
    class MyRabbitClient
    {
        private const string host = "localhost";
        private const int port = 32773; //Mapped on Docker to 5672/tcp

        public static void TestRabbit() {
            SayHello();
            System.Threading.Thread.Sleep(200);
            ListenHello();
        }

        public static void SayHello(string text = "Hello World!") {
            var factory = new ConnectionFactory()
            {
                HostName = host
                ,Port = port
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = text;
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "hello",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }
        }

        public static void ListenHello()
        {
            var factory = new ConnectionFactory() {
                HostName = host
                ,Port = port 
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);
                };

                channel.BasicConsume(queue: "hello",
                                     autoAck: true,
                                     consumer: consumer);
                
                //not sure how to wait for the Received event
                System.Threading.Thread.Sleep(200);

            }
        }
    }
}
