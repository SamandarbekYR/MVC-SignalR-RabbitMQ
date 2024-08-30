using MVCLearn.DataAcess.Interfaces.Messages;
using MVCLearn.DataAcess.Interfaces.UsersMessages;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using ReceiverApp.WebApi.Interfaces.RabbitMQ;
using System.Text;
using MVCLearn.DTOs;
using Newtonsoft.Json;
using MVCLearn.Models;
using MVCLearn.Interfaces.Messages;
using Microsoft.Extensions.Configuration;

namespace ReceiverApp.WebApi.Services.RabbitMQ
{
    public class RabbitMQConsumerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IModel _channel;
        private readonly IConfigurationSection _config;
        private IConnection _connection;

        public RabbitMQConsumerService(IConfiguration config, IServiceProvider serviceProvider)
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
                    var usersMessageRepository = scope.ServiceProvider.GetRequiredService<IUsersMessagesRepository>();
                    var messageRepository = scope.ServiceProvider.GetRequiredService<IMessageRepository>();
                    var emailSenderService = scope.ServiceProvider.GetRequiredService<IEmailSenderService>();

                    var body = ea.Body.ToArray();
                    var messageContent = Encoding.UTF8.GetString(body);

                    SendMessageRequestDto dto = JsonConvert.DeserializeObject<SendMessageRequestDto>(messageContent)!;

                    // Xabarni bazaga saqlash
                    var message = new Message { Content = dto.messageBody, SendAt = DateTime.UtcNow.AddHours(5) };
                    Guid messageId = await messageRepository.Add(message);

                    foreach (var userId in dto.users)
                    {
                        var userMessage = new UserMessage { UserId = userId.Id, MessageId = messageId };
                        await usersMessageRepository.Add(userMessage);

                        MessageDto messageDto = new()
                        {
                            Title = "Boss's message\n",
                            Body = dto.messageBody,
                            To = userId.Email
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
