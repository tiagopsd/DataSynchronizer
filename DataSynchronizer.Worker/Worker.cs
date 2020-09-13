using System;
using System.Threading;
using System.Threading.Tasks;
using DataSynchronizer.Aplication.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Abstraction;

namespace DataSynchronizer.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly PublisherService _publisherService;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, PublisherService publisherService, IConfiguration configuration)
        {
            _logger = logger;
            _publisherService = publisherService;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var numberMessagesSend = _configuration?.GetValue<short>("NumberMessagesSend") ?? 10;
                    var delayLoop = _configuration?.GetValue<short>("DelayLoop") ?? 5000;

                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                    _publisherService.Process(numberMessagesSend);
                    await Task.Delay(delayLoop, stoppingToken);
                }
                catch (Exception error)
                {
                    _logger.LogError(error.GetBaseException(), $"Erro ao precossar publicações. " +
                        $"StackTrace: {error.GetBaseException().StackTrace}");
                }
            }
        }
    }
}
