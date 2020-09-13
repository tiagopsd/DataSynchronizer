using Newtonsoft.Json;
using RabbitMQ.Abstraction;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQ.Implamentation
{
    public class RabbitListener<T> : IRabbitListener<T> where T : class
    {
        public RabbitListener(IRabbitMQConfiguration rabbitConfiguration)
        {
            rabbitConfiguration.ConfigureListenerType<T>(Consumer);
        }

        public EventListener<T> EventListener { get; set; }
        private void Consumer(object sender, BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var json = JsonConvert.DeserializeObject<T>(message);
            EventListener.Invoke(json);
        }
    }
}
