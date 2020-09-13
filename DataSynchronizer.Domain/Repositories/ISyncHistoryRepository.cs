using DataSynchronizer.Domain.Entities;
using DataSynchronizer.Domain.Enumerations;
using DataSynchronizer.Domain.Interfaces;
using DataSynchronizer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataSynchronizer.Domain.Repositories
{
    public interface ISyncHistoricRepository : IRepository<SyncHistoric>
    {
        List<SyncHistoric> GetNewHistoric(short quantitySearch);
        string GetJsonObject(string tableName, Guid objectGuid);
        Result InsertJsonObject(string tableName, string objectJson, Guid objectGuid);
        Result UpdateJsonObject(string tableName, string objectJson, Guid objectGuid);
        Result DeleteObject(string tableName, Guid objectGuid);
        SyncHistoric GetBySyncGuid(Guid syncGuid);
    }
}
