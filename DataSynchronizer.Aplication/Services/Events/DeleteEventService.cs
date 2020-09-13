using DataSynchronizer.Aplication.Services.Events;
using DataSynchronizer.Domain.Entities;
using DataSynchronizer.Domain.Models;
using DataSynchronizer.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataSynchronizer.Aplication.Services
{
    public class DeleteEventService : EventBaseService
    {
        public DeleteEventService(ISyncHistoricRepository syncHistoricRepository)
            : base(syncHistoricRepository)
        {
        }

        protected override Result Operate(string tableName, string objectJson, Guid objectGuid)
        {
            return _syncHistoricRepository.DeleteObject(tableName, objectGuid);
        }

        protected override Result Validate(HistoricModel historicModel)
        {
            return ValidateExistObject(historicModel.TableName, historicModel.ObjectGuid);
        }
    }
}
