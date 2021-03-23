using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using Common.MessageContracts;
using Common.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace WcfTransmitter.Service.WcfEndpoint
{
    public class OutcomesChangesReceiver : IDisposable
    {
        private IConnection _connection;
        private readonly string _hostname;
        private readonly string _password;
        private readonly string _username;
        private string _queueName;
        private IModel _channel;
        private readonly Action<IEnumerable<Outcome>> _outcomeHandle;
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public OutcomesChangesReceiver(Action<IEnumerable<Outcome>> outcomeHandler)
        {
            _hostname = ConfigurationManager.AppSettings["RabbitMqHostname"];
            _username = ConfigurationManager.AppSettings["RabbitMqUser"];
            _password = ConfigurationManager.AppSettings["RabbitMqPassword"];

            _outcomeHandle = outcomeHandler;

            InitializeListener();
        }

        private void InitializeListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password,
                DispatchConsumersAsync = true
            };
            
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(MessageProperties.Outcomes.Exchange, ExchangeType.Fanout);
            
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(_queueName, MessageProperties.Outcomes.Exchange, "");
        }
        
        public void BeginReceive()
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += ConsumerOnReceived;
            
            _channel.BasicConsume(_queueName, autoAck: true, consumer);
        }
        
        
        private Task ConsumerOnReceived(object? sender, BasicDeliverEventArgs e)
        {
            try
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
            
                var outcomes = JsonConvert.DeserializeObject<OutcomesMessage>(message);
                _outcomeHandle(outcomes.Outcomes);
                return Task.CompletedTask;
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }
        }

        public void EndReceive()
        {
            _channel.Close();
            _connection.Close();
        }

        public void Dispose()
        {
            EndReceive();
        }
    }
}