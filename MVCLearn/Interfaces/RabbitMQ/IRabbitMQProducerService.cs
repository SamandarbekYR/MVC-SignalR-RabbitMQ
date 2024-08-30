namespace MVCLearn.Interfaces.RabbitMQ
{
    public interface IRabbitMQProducerService
    {
        public void SendMessage(string message);
    }
}
