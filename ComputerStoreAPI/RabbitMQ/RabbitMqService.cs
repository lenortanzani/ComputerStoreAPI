using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStoreAPI.RabbitMQ
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly IConfiguration _configuration;
        public RabbitMqService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void SendMessage(string message)
        {
            var factory = new ConnectionFactory() { Uri = new Uri(_configuration.GetConnectionString("RabbitMq")) };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _configuration["RabbitMq:DbChangesQueue"],
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(message); // we can send any serialized object

                channel.BasicPublish(exchange: "",
                                     routingKey: _configuration["RabbitMq:DbChangesQueue"],
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
