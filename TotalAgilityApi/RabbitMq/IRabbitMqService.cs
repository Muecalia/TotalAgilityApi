namespace TotalAgilityApi.RabbitMq
{
    public interface IRabbitMqService
    {
        void SendMessage<T>(T message, string queue);
    }
}
