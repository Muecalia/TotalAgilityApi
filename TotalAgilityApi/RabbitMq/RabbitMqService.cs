using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace TotalAgilityApi.RabbitMq
{
    public class RabbitMqService : IRabbitMqService
    {
        private RabbitMqSettings _settings { get; }

        public RabbitMqService(IOptions<RabbitMqSettings> settings)
        {
            _settings = settings.Value;
        }

        public void SendMessage<T>(T message, string queue)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _settings.HostName,
                    UserName = _settings.UserName,
                    Password = _settings.Password
                };

                using var connection = factory.CreateConnection();

                using var channel = connection.CreateModel();
                //channel.QueueDeclare(queue: _settings.QueueName,
                channel.QueueDeclare(queue: queue,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var json = JsonSerializer.Serialize(message);

                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: "",
                                     //routingKey: _settings.QueueName,
                                     routingKey: queue,
                                     basicProperties: null,
                                     body: body);

            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
