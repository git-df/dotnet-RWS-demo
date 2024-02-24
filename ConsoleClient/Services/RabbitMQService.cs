using API.Models;
using RabbitMQ.Client;
using System.Text;

namespace ConsoleClient.Services
{
    public class RabbitMQService
    {
        private readonly RabbitMQOptions _rabbitmqOptions;
        private readonly ConnectionFactory _factory;

        public RabbitMQService()
        {
            _rabbitmqOptions = new RabbitMQOptions()
            {
                HostName = "localhost",
                UserName = "user",
                Password = "Haslo123!",
                Exchange = "EventsExchange",
                RoutingKey = "Events"
            };

            _factory = new ConnectionFactory()
            {
                HostName = _rabbitmqOptions.HostName,
                UserName = _rabbitmqOptions.UserName,
                Password = _rabbitmqOptions.Password,
            };
        }

        public bool SendMessage(EventMessage message)
        {
            try
            {
                using (var connection = _factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(
                        exchange: _rabbitmqOptions.Exchange,
                        type: ExchangeType.Fanout,
                        durable: true,
                        autoDelete: false,
                        arguments: null);


                    var body = Encoding.UTF8.GetBytes(message.ToJson());

                    channel.BasicPublish(
                        exchange: _rabbitmqOptions.Exchange,
                        routingKey: _rabbitmqOptions.RoutingKey,
                        basicProperties: null,
                        body: body);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
