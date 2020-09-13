namespace RabbitMQ.Abstraction

{
    public delegate void EventListener<T>(T Modelo);
    public interface IRabbitListener<T> where T : class
    {
        EventListener<T> EventListener { get; set; }
    }
}