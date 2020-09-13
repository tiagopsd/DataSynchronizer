using Newtonsoft.Json;
using RabbitMQ.Abstraction;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Implamentation
{
    public class RabbitPublish<T> : IRabbitPublish<T> where T : class
    {
        private List<IChannel> _channelsType;
        public RabbitPublish(IRabbitMQConfiguration rabbitConfiguration)
        {
            _channelsType = rabbitConfiguration
                .GetChannelsTypes<T>(TypeChannel.Publish);
        }

        private void Publish(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channelsType.ForEach(d =>
            {
                d.ChannelRabbit.BasicPublish(exchange: "",
                                 routingKey: d.Name,
                                 basicProperties: null,
                                 body: body);
            });
        }

        public void Publish(T model)
        {
            var json = JsonConvert.SerializeObject(model);
            Publish(json);
        }
    }
}
