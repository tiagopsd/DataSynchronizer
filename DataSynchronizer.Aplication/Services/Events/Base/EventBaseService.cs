using DataSynchronizer.Domain.Entities;
using DataSynchronizer.Domain.Models;
using DataSynchronizer.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Transactions;

namespace DataSynchronizer.Aplication.Services.Events
{
    public abstract class EventBaseService
    {
        protected readonly ISyncHistoricRepository _syncHistoricRepository;
        public EventBaseService(ISyncHistoricRepository syncHistoricRepository)
        {
            _syncHistoricRepository = syncHistoricRepository;
        }

        protected SyncHistoric GetCurrentHistoric(Guid syncGuid)
        {
            return _syncHistoricRepository.GetBySyncGuid(syncGuid);
        }

        public Result Process(HistoricModel historicModel)
        {
            var currentHistoric = GetCurrentHistoric(historicModel.SyncGuid);
            if (currentHistoric != null)
                return Result.BuildSucess();

            var result = Validate(historicModel);
            if (!result.Sucesso)
                return result;

            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.RepeatableRead,
                Timeout = TimeSpan.MaxValue
            }))
            {
                var resultJson = Operate(historicModel.TableName, historicModel.JsonObject, historicModel.ObjectGuid);
                var resultHistoric = InsertHistoric(historicModel);

                if (!resultJson.Sucesso || !resultHistoric.Sucesso)
                    return Result.BuildError("Erro ao gravar " + resultJson);

                transaction.Complete();
                return Result.BuildSucess();
            }
        }

        protected abstract Result Operate(string tableName, string objectJson, Guid objectGuid);

        protected abstract Result Validate(HistoricModel historicModel);

        protected Result InsertHistoric(HistoricModel historicModel)
        {
            try
            {
                var historic = new SyncHistoric
                {
                    DateTimeSync = historicModel.DateTimeSync,
                    ObjectGuid = historicModel.ObjectGuid,
                    StateSync = Domain.Enumerations.StateSync.synced,
                    SyncGuid = historicModel.SyncGuid,
                    TypeSync = historicModel.TypeSync,
                    TableName = historicModel.TableName
                };
                _syncHistoricRepository.Add(historic);

                var result = _syncHistoricRepository.Save();
                if (result > 0)
                    return Result.BuildSucess();

                return Result.BuildError("Erro ao gravar histórico!");
            }
            catch (Exception error)
            {
                return Result.BuildError("Erro ao gravar histórico", error);
            }
        }

        protected Result ValidateExistObject(string tableName, Guid objectGuid)
        {
            var json = _syncHistoricRepository.GetJsonObject(tableName, objectGuid);
            if (string.IsNullOrWhiteSpace(json))
                return Result.BuildError("Objeto não encontrado!");

            return Result.BuildSucess();
        }
    }
}
