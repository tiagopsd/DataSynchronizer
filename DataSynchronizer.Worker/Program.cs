using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DataSynchronizer.Aplication.Services;
using DataSynchronizer.Domain.Repositories;
using DataSynchronizer.Infra;
using DataSynchronizer.Infra.RabbitMQ;
using DataSynchronizer.Infra.Repositories;
using DataSynchronizer.Infra.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.InjectionOfDependency;
using Serilog;
using Serilog.Formatting;
using Serilog.Formatting.Compact;

namespace DataSynchronizer.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
               .Build()
               .CreateInstace<RabbitConfiguration>()
               .CreateInstace<ListenerService>()
               .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();

                    services.AddSingleton<PublisherService, PublisherService>();
                    services.AddSingleton<ListenerService, ListenerService>();

                    services.AddScoped<ISyncHistoricRepository, SyncHistoryRepository>();
                    services.AddScoped<Command>();
                    services.AddScoped<DeleteEventService, DeleteEventService>();
                    services.AddScoped<InsertEventService, InsertEventService>();
                    services.AddScoped<UpdateEventService, UpdateEventService>();
                   
                    services.AddRabbitMQ<RabbitConfiguration>();
                    services.AddDbContext<Context>(ServiceLifetime.Scoped);
                    services.AddLogging(ConfigureLogger);
                });

        private static void ConfigureLogger(ILoggingBuilder loggingBuilder)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            var logger = new LoggerConfiguration()
                .WriteTo.File($"Logs/log-{DateTime.Now.Date:dd.MM.yyy}.txt")
                .CreateLogger();

            loggingBuilder.AddSerilog(logger);
        }
    }
}
