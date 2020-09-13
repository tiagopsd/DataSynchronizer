using DataSynchronizer.Domain.Entities;
using DataSynchronizer.Domain.Models;
using DataSynchronizer.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataSynchronizer.Aplication.Services
{
    public class PublisherService
    {
        private readonly IRabbitPublish<HistoricModel> _rabbitPublishObject;
        private readonly IRabbitPublish<StatusModel> _rabbitPublishStatus;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PublisherService> _logger;

        public PublisherService(IRabbitPublish<HistoricModel> rabbitPublishObject,
             IRabbitPublish<StatusModel> rabbitPublishStatus,
            IServiceProvider serviceProvider,
            ILogger<PublisherService> logger)
        {
            _serviceProvider = serviceProvider;
            _rabbitPublishObject = rabbitPublishObject;
            _rabbitPublishStatus = rabbitPublishStatus;
            _logger = logger;
        }

        public void Process(short numberMessagesSend)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var syncHistoricRepository = scope.ServiceProvider.GetRequiredService<ISyncHistoricRepository>();
                var historical = syncHistoricRepository.GetNewHistoric(numberMessagesSend);
                historical.ForEach(d => PublishHistoric(d, syncHistoricRepository));
            }
        }

        private void PublishHistoric(SyncHistoric syncHistoric, ISyncHistoricRepository syncHistoricRepository)
        {
            try
            {
                string json = syncHistoricRepository.GetJsonObject(syncHistoric.TableName, syncHistoric.ObjectGuid);
                _rabbitPublishObject.Publish(new HistoricModel
                {
                    DateTimeSync = syncHistoric.DateTimeSync,
                    SyncGuid = syncHistoric.SyncGuid,
                    ObjectGuid = syncHistoric.ObjectGuid,
                    JsonObject = json,
                    TypeSync = syncHistoric.TypeSync,
                    TableName = syncHistoric.TableName
                });
            }
            catch (Exception error)
            {
                _logger.LogError($"Erro ao publicar histórico. {syncHistoric}", error);
            }
        }

        internal void PublishUpdateStatus(Guid syncGuid)
        {
            try
            {
                _rabbitPublishStatus.Publish(new StatusModel
                {
                    SyncGuid = syncGuid
                });
            }
            catch (Exception error)
            {
                _logger.LogError($"Erro ao publicar status.", error);
            }
        }
    }
}
