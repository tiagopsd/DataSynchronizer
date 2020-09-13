using DataSynchronizer.Aplication.Services.Events;
using DataSynchronizer.Domain.Entities;
using DataSynchronizer.Domain.Models;
using DataSynchronizer.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace DataSynchronizer.Aplication.Services
{
    public class InsertEventService : EventBaseService
    {
        public InsertEventService(ISyncHistoricRepository syncHistoricRepository)
            : base(syncHistoricRepository)
        {
        }

        protected override Result Operate(string tableName, string objectJson, Guid objectGuid)
        {
           return _syncHistoricRepository.InsertJsonObject(tableName, objectJson, objectGuid);
        }

        protected override Result Validate(HistoricModel historicModel)
        {
            return Result.BuildSucess();
        }
    }
}
