using RabbitMQ.Abstraction;
using RabbitMQ.Client;
using System;

namespace RabbitMQ.Implamentation
{
    public class Channel : IChannel
    {
        public IModel ChannelRabbit { get; set; }
        public Type TypeModel { get; set; }
        public string Name { get; set; }
        public TypeChannel Type { get; set; }

        public Channel()
        {

        }

        public Channel(IModel channelRabbit, Type typeModel, string name, TypeChannel type)
        {
            ChannelRabbit = channelRabbit;
            TypeModel = typeModel;
            Name = name;
            Type = type;
        }
    }
}
