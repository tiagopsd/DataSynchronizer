using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;

namespace RabbitMQ.Abstraction
{
    public interface IRabbitMQConfiguration
    {
        void AddChannel<T>(string channelName, TypeChannel type);
        List<IChannel> GetChannelsTypes<T>(TypeChannel type);
        void ConfigureListenerType<T>(EventHandler<BasicDeliverEventArgs> eventConsumer);
    }
}
