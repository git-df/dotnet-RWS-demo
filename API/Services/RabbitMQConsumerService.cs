
using API.Hubs;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace API.Services
{
    public class RabbitMQConsumerService : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IHubContext<EventsHub, IEventsClient> _hubContext;
        private readonly RabbitMQOptions _rabbitmqOptions;

        public RabbitMQConsumerService(ILogger<RabbitMQConsumerService> logger, IHubContext<EventsHub,
            IEventsClient> hubContext, IOptions<RabbitMQOptions> rabbitmqOptions)
        {
            _rabbitmqOptions = rabbitmqOptions.Value;
            _hubContext = hubContext;

            var factory = new ConnectionFactory()
            {
                HostName = _rabbitmqOptions.HostName,
                UserName = _rabbitmqOptions.UserName,
                Password = _rabbitmqOptions.Password,
            };


            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: _rabbitmqOptions.RoutingKey,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);

                var message = EventMessage.FromJson(messageJson);

                await _hubContext.Clients.Group(message.Login).ReceiveEvents(message);

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(_rabbitmqOptions.RoutingKey, false, consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }
}
