namespace RabbitMQ.Abstraction
{
    public interface IRabbitPublish<T> where T : class
    {
        void Publish(T model);
    }
}
