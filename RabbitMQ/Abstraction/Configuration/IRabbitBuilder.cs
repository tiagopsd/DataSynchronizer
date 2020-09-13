using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Abstraction.Configuration
{
    public interface IRabbitBuilder
    {
        void AddChannel<T>(string channelName, TypeChannel type);
    }
}
