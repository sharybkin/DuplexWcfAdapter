using System;
using System.Text;
using BettingLine.Service.Options;
using Common.MessageContracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace BettingLine.Service.BackgroundService
{
    public class MessagePublisher : IMessagePublisher
    {
        private IConnection _connection;
        private readonly string _hostname;
        private readonly string _password;
        private readonly string _username;

        private readonly ILogger<MessagePublisher> _logger;
        
        private IConnection Connection => GetConnection();

        public MessagePublisher(IOptions<RabbitMqConfiguration> rabbitMqOptions, ILogger<MessagePublisher> logger)
        {
            _logger = logger;
            _hostname = rabbitMqOptions.Value.Hostname;
            _username = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;
        }
        
        public void Publish<T>(string exchange, T message) where T : Message
        {
            try
            {
                using var channel = Connection.CreateModel();
                channel.ExchangeDeclare(exchange, ExchangeType.Fanout);

                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: exchange, routingKey: "", basicProperties: null, body: body);

                _logger.LogInformation($"MessageId {message.Id} was sent to {exchange}.");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Message publication was failed");
            }
        }

        private void CreateConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password
            };
            _connection = factory.CreateConnection();
        }
        

        private IConnection GetConnection()
        {
            if (_connection != null)
                return _connection;
            
            CreateConnection();
            return _connection;
        }
    }
}