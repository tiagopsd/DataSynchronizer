using RabbitMQ.Client;

namespace RabbitMQ.Implamentation
{
    public class RabbitConnection
    {
        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        public RabbitConnection()
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = "localhost"
            };
        }

        public void Connect()
        {
            _connection = _connectionFactory.CreateConnection();
        }

        public IModel CreateChannel()
        {
            return _connection.CreateModel();
        }
    }
}
