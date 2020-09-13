using RabbitMQ.Abstraction.Configuration;

namespace RabbitMQ.Abstraction
{
    public interface IRabbitConfiguration
    {
        void AddChannel<T>(string channelName, TypeChannel type);
    }
}
