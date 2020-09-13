using DataSynchronizer.Domain.Entities;
using DataSynchronizer.Domain.Enumerations;
using DataSynchronizer.Domain.Models;
using DataSynchronizer.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Abstraction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataSynchronizer.Aplication.Services
{
    public class ListenerService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly PublisherService _publisherService;
        private readonly ILogger<ListenerService> _logger;

        public ListenerService(IRabbitListener<HistoricModel> rabbitListenerHistoric,
            IRabbitListener<StatusModel> rabbitListenerStatus,
            IServiceProvider serviceProvider,
            PublisherService publisherService,
            ILogger<ListenerService> logger)
        {
            rabbitListenerHistoric.EventListener += EventListenHistoric;
            rabbitListenerStatus.EventListener += EventListenStatus;
            _serviceProvider = serviceProvider;
            _publisherService = publisherService;
            _logger = logger;
        }

        private void EventListenHistoric(HistoricModel historicModel)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var result = ProcessEvent(historicModel, scope);
                    if (result.Sucesso)
                        _publisherService.PublishUpdateStatus(historicModel.SyncGuid);
                }
            }
            catch (Exception error)
            {
                _logger.LogError($"Erro ao processar histórico. {historicModel}", error);
            }
        }

        private Result ProcessEvent(HistoricModel objectModel, IServiceScope scope)
        {
            switch (objectModel.TypeSync)
            {
                case TypeSync.Insert:
                    return GetService<InsertEventService>().Process(objectModel);
                case TypeSync.Update:
                    return GetService<UpdateEventService>().Process(objectModel);
                case TypeSync.Delete:
                    return GetService<DeleteEventService>().Process(objectModel);
                default:
                    return Result.BuildError($"Tipo de sincronização não encontrado! {objectModel}");
            }

            T GetService<T>() => scope.ServiceProvider.GetService<T>();
        }

        private void EventListenStatus(StatusModel statusModel)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var syncHistoricRepository = scope.ServiceProvider.GetService<ISyncHistoricRepository>();
                    var syncHistoric = syncHistoricRepository.GetBySyncGuid(statusModel.SyncGuid);
                    if (syncHistoric != null)
                    {
                        syncHistoric.StateSync = StateSync.synced;
                        syncHistoricRepository.Save();
                    }
                }
            }
            catch (Exception error)
            {
                _logger.LogError($"Erro ao atualizar status. " +
                    $"SyncHistoric: {statusModel.SyncGuid}", error);
            }
        }
    }
}
