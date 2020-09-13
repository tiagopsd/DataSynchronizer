using DataSynchronizer.Domain.Enumerations;
using DataSynchronizer.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataSynchronizer.Domain.Entities
{
    public class SyncHistoric : IEntity<long>
    {
        public long Id { get; set; }
        public DateTime DateTimeSync { get; set; }
        public Guid ObjectGuid { get; set; }
        public Guid SyncGuid { get; set; }
        public TypeSync TypeSync { get; set; }
        public StateSync StateSync { get; set; }
        public string TableName { get; set; }

        public override string ToString()
        {
            return $"ObjectTableName: {TableName} ObjectGuid: {ObjectGuid} SyncGuid: {SyncGuid} TypeSync {TypeSync}";
        }
    }
}
