using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Abstraction;
using RabbitMQ.Abstraction.Configuration;
using RabbitMQ.Implamentation;
using System;

namespace RabbitMQ.InjectionOfDependency
{
    public static class InjectRabbitMQ
    {
        public static IServiceCollection AddRabbitMQ<T>(this IServiceCollection services) where T : class
        {
            services.AddSingleton(typeof(IRabbitMQConfiguration), typeof(RabbitMQConfiguration));
            services.AddSingleton<T>();
            services.AddSingleton(typeof(IRabbitPublish<>), typeof(RabbitPublish<>));
            services.AddSingleton(typeof(IRabbitListener<>), typeof(RabbitListener<>));
            return services;
        }

        public static IHost CreateInstace<T>(this IHost host)
        {
            host.Services.GetService<T>();
            return host;
        }
    }
}
