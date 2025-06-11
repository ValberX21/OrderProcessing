using System.Text;
using Newtonsoft.Json;
using OrderProcessing.Domain.Entity;
using RabbitMQ.Client;

namespace OrderProcessing.Application.Interface
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly IConnection _con;
        private readonly IModel _channel;

        public RabbitMqService()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "rabbitmq",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            _con = factory.CreateConnection();
            _channel = _con.CreateModel();

            _channel.QueueDeclare(
                queue: "order-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }
        public Task SendMessage(OrderDto message)
        {
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish(
                exchange: "",
                routingKey: "order-queue",
                basicProperties: null,
                body: body
            );

            return Task.CompletedTask;
        }
    }
}