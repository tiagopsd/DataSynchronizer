using DataSynchronizer.Domain.Enumerations;
using System;
using System.Collections.Generic;

namespace DataSynchronizer.Domain.Models
{
    public class HistoricModel
    {
        public string JsonObject { get; set; }
        public Guid ObjectGuid { get; set; }
        public DateTime DateTimeSync { get; set; }
        public Guid SyncGuid { get; set; }
        public TypeSync TypeSync { get; set; }
        public string TableName { get; set; }

        public override string ToString()
        {
            return $"ObjectGuid: {ObjectGuid} GuidSync: {SyncGuid} TypeSync: {TypeSync} Json: {JsonObject}";
        }
    }
}
