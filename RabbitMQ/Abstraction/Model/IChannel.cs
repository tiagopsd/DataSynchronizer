using RabbitMQ.Client;
using System;

namespace RabbitMQ.Abstraction
{
    public interface IChannel
    {
        IModel ChannelRabbit { get; set; }
        Type TypeModel { get; set; }
        string Name { get; set; }
        TypeChannel Type { get; set; }
    }
}
