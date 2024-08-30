using MVCLearn.Interfaces.RabbitMQ;
using RabbitMQ.Client;
using System.Text;

namespace MVCLearn.Services.RabbitMQ
{
    public class RabbitMQProducerService : IRabbitMQProducerService
    {
        private readonly IConfigurationSection _config;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQProducerService(IConfiguration configuration)
        {
            this._config = configuration.GetSection("MessageBroker");
            var factory = new ConnectionFactory()
            {
                HostName = _config["Host"],
                Port = int.Parse(_config["Port"]!),
                UserName = _config["Username"],
                Password = _config["Password"]
            };
             _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _config["Queue"],
                                  durable: true, exclusive: false,
                                  autoDelete: false, arguments: null);

            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
        }
        public void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "",
                                 routingKey: "MessageQueue",
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine("Sent {0}", message);
        }
        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
