using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

public class MessageConsumerService
{
    public void StartListening()
    {
        var factory = new ConnectionFactory()
        {
            HostName = Environment.GetEnvironmentVariable("RabbitMq__HostName") ?? "localhost",
            Port = int.Parse(Environment.GetEnvironmentVariable("RabbitMq__Port") ?? "5672"),
            UserName = Environment.GetEnvironmentVariable("RabbitMq__UserName") ?? "guest",
            Password = Environment.GetEnvironmentVariable("RabbitMq__Password") ?? "guest",
        };

        int retries = 5;
        while (retries > 0)
        {
            try
            {
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                var queueName = Environment.GetEnvironmentVariable("RabbitMq__QueueName") ?? "order-queue";

                channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"[x] Received: {message}");

                    try
                    {
                        // TODO: your logic to process message (e.g., save to DB)

                        // Acknowledge only after successful processing
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[!] Error processing message: {ex.Message}");

                        // Optionally: reject and requeue or discard
                        // channel.BasicNack(ea.DeliveryTag, false, true); // true to requeue
                    }
                };


                channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

                Console.WriteLine("Listening for message. Press Ctrl+C to exit.");
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RabbitMQ not ready yet: {ex.Message}");
                retries--;
                Thread.Sleep(3000);
            }
        }
    }

}