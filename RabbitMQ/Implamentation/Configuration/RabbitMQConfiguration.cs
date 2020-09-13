using RabbitMQ.Abstraction;
using RabbitMQ.Abstraction.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RabbitMQ.Implamentation
{
    public class RabbitMQConfiguration : IRabbitMQConfiguration
    {
        RabbitConnection _rabbitConnection;
        public List<IChannel> _channelsTypes;
        public RabbitMQConfiguration()
        {
            _rabbitConnection = new RabbitConnection();
            _rabbitConnection.Connect();
            _channelsTypes = new List<IChannel>();
        }

        public void AddChannel<T>(string channelName, TypeChannel type)
        {
            var channelType = new Channel(CreateChannel(channelName), typeof(T), channelName, type);
            _channelsTypes.Add(channelType);
        }

        private IModel CreateChannel(string name)
        {
            var channel = _rabbitConnection.CreateChannel();
            channel.QueueDeclare(queue: name,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
            return channel;
        }

        public List<IChannel> GetChannelsTypes<T>(TypeChannel type)
        {
            return _channelsTypes
                .Where(d => d.TypeModel == typeof(T) && d.Type == type)
                .ToList();
        }

        private EventingBasicConsumer GetEventConsumer(IModel channel)
        {
            return new EventingBasicConsumer(channel);
        }

        public void ConfigureListenerType<T>(EventHandler<BasicDeliverEventArgs> eventConsumer)
        {
            var channelsType = GetChannelsTypes<T>(TypeChannel.Listener);
            channelsType.ForEach(d =>
            {
                var consumer = GetEventConsumer(d.ChannelRabbit);
                consumer.Received += eventConsumer;
                d.ChannelRabbit.BasicConsume(queue: d.Name,
                                    autoAck: true,
                                    consumer: consumer);
            });
        }
    }
}
