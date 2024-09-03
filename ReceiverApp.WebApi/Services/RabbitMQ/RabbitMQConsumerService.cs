using MVCLearn.DataAcess.Interfaces.Messages;
using MVCLearn.DTOs;
using MVCLearn.Interfaces.Messages;
using MVCLearn.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ReceiverApp.WebApi.Services.RabbitMQ
{
    public class RabbitMQConsumerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IModel _channel;
        private IConnection _connection;

        public RabbitMQConsumerService(IConfiguration config,
                                       IServiceProvider serviceProvider )
                                      
        {
            _serviceProvider = serviceProvider;
            // _config = config.GetSection("MessageBroker");

            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "MessageQueue",
                                  durable: true, exclusive: false,
                                  autoDelete: false, arguments: null);
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            StartListening();
            return Task.CompletedTask;
        }

        private void StartListening()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _messageRepository = scope.ServiceProvider.GetRequiredService<IMessageRepository>();
                    var emailSenderService = scope.ServiceProvider.GetRequiredService<IEmailSenderService>();

                    var body = ea.Body.ToArray();
                    var messageContent = Encoding.UTF8.GetString(body);

                    SendMessageRequestDto dto = JsonConvert.DeserializeObject<SendMessageRequestDto>(messageContent)!;


                    foreach (var user in dto.users)
                    {
                        var message = new Message
                        {
                            SenderId = dto.SenderId,
                            ReceiverId = user.Id,
                            MessageContent = dto.messageBody,
                            SendTime = DateTime.UtcNow.AddHours(5),
                            IsRead = false
                        };

                        await _messageRepository.Add(message);

                        MessageDto messageDto = new()
                        {
                            Title = "Boss's message\n",
                            Body = dto.messageBody,
                            To = user.Email
                        };
                        await emailSenderService.SendEmailAsync(messageDto);
                    }
                }
                _channel.BasicAck(ea.DeliveryTag, multiple: false);
            };
            _channel.BasicConsume(queue: "MessageQueue",
                                  autoAck: false,
                                  consumer: consumer);
        }
        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}