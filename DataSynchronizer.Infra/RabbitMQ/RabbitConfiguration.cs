
using DataSynchronizer.Domain.Models;
using Microsoft.Extensions.Configuration;
using RabbitMQ;
using RabbitMQ.Abstraction;
using System.Collections.Generic;

namespace DataSynchronizer.Infra.RabbitMQ
{
    public class RabbitConfiguration
    {
        public RabbitConfiguration(IRabbitMQConfiguration rabbitConfiguration, IConfiguration configuration)
        {
            Configure(rabbitConfiguration, configuration);
        }

        public void Configure(IRabbitMQConfiguration rabbitConfiguration, IConfiguration configuration)
        {
            var listenerObjectModel = configuration.GetValue<string>("ListenerHistoric") ?? "ListenerObjectModel";
            var publisherObjectModel = configuration.GetValue<string>("PublisherHistoric") ?? "PublisherObjectModel";
            
            rabbitConfiguration.AddChannel<HistoricModel>(listenerObjectModel, TypeChannel.Listener);
            rabbitConfiguration.AddChannel<HistoricModel>(publisherObjectModel, TypeChannel.Publish);

            var listenerStatusModel = configuration.GetValue<string>("ListenerStatus") ?? "ListenerObjectModel";
            var publisherStatusModel = configuration.GetValue<string>("PublisherStatus") ?? "PublisherObjectModel";

            rabbitConfiguration.AddChannel<StatusModel>(listenerStatusModel, TypeChannel.Listener);
            rabbitConfiguration.AddChannel<StatusModel>(publisherStatusModel, TypeChannel.Publish);
        }
    }
}
