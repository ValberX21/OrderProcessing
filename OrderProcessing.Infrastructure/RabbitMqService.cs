using Microsoft.Extensions.Configuration; // Add this using directive
using Newtonsoft.Json;
using OrderProcessing.Application.Interface;
using OrderProcessing.Domain.Entity;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Infrastructure
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly IConnection _con;
        private readonly IModel _channel;
        private readonly string _queueName; // To store the queue name

        public RabbitMqService(IConfiguration configuration) // Inject IConfiguration
        {
            // Retrieve RabbitMQ configuration from appsettings.json or environment variables
            var rabbitMqHost = configuration["RabbitMq:Host"];
            var rabbitMqPort = int.Parse(configuration["RabbitMq:Port"]);
            var rabbitMqUser = configuration["RabbitMq:UserName"];
            var rabbitMqPassword = configuration["RabbitMq:Password"];
            _queueName = configuration["RabbitMq:Queue"]; // Get the queue name

            var factory = new ConnectionFactory()
            {
                HostName = rabbitMqHost,
                Port = rabbitMqPort,
                UserName = rabbitMqUser,
                Password = rabbitMqPassword
            };

            _con = factory.CreateConnection();
            _channel = _con.CreateModel();

            _channel.QueueDeclare(
                queue: _queueName, // Use the configured queue name
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }

        public Task SendMessageAsync(string queueName, OrderDto message)
        {
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                basicProperties: null,
                body: body
            );

            return Task.CompletedTask;
        }
    }
}