using DataSynchronizer.Domain.Entities;
using DataSynchronizer.Domain.Enumerations;
using DataSynchronizer.Domain.Models;
using DataSynchronizer.Domain.Repositories;
using DataSynchronizer.Infra.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace DataSynchronizer.Infra.Repositories
{
    public class SyncHistoryRepository : Repository<SyncHistoric>, ISyncHistoricRepository
    {
        private readonly Command _command;
        public SyncHistoryRepository(Context context, Command command)
            : base(context)
        {
            _command = command;
        }

        public List<SyncHistoric> GetNewHistoric(short quantitySearch)
        {
            return Set.Where(d => d.StateSync == StateSync.Pending)
                .Take(quantitySearch).ToList();
        }

        public Result InsertJsonObject(string tableName, string objectJson, Guid objectGuid)
        {
            try
            {
                var columnsModel = JsonConvert.DeserializeObject<DataTable>(objectJson)
                    .GetColumnsModel();

                string command = "";

                var triggerName = GetTriggerName(tableName);
                command += EnableDisableTrigger(tableName, triggerName, "disable");

                command += $"Insert into {tableName} ({string.Join(",", columnsModel.Names)}) values " +
                   $"({string.Join(",", columnsModel.Names.Select(d => "@" + d))});";

                command += EnableDisableTrigger(tableName, triggerName, "enable");

                _command.CreateCommand(command);

                for (int i = 0; i < columnsModel.Names.Count; i++)
                    _command.AddParameter("@" + columnsModel.Names[i], columnsModel.Values[i]);

                var linesAffected = _command.ExecuteNonQuery();
                _command.CloseConneciton();

                if (linesAffected > 0)
                    return Result.BuildSucess();

                return Result.BuildError($"Erro ao inserir registro. " +
                        $"Tabela: {tableName} " +
                        $"Guid: {objectGuid} " +
                        $"Json: {objectJson}");
            }
            catch (Exception error)
            {
                return Result.BuildError($"Erro ao inserir registro. " +
                        $"Tabela: {tableName} " +
                        $"Guid: {objectGuid} " +
                        $"Json: {objectJson} ", error);
            }
        }

        public string GetJsonObject(string tableName, Guid objectGuid)
        {
            var command = $"Select * from {tableName} where Guid = @Guid";

            _command.CreateCommand(command);
            _command.AddParameter("@Guid", objectGuid);

            var ret = _command.ExecuteReader();

            DataTable dataTable = new DataTable();
            dataTable.Load(ret);
            _command.CloseConneciton();

            var json = JsonConvert.SerializeObject(dataTable);
            return json;
        }

        public Result UpdateJsonObject(string tableName, string objectJson, Guid objectGuid)
        {
            try
            {
                var columnsModel = JsonConvert.DeserializeObject<DataTable>(objectJson)
                    .GetColumnsModel();

                var triggerName = GetTriggerName(tableName);
                string command = EnableDisableTrigger(tableName, triggerName, "disable");

                command += $"update {tableName} set ";
                command += string.Join(",", columnsModel.Names.Select(d => $"{d} = @{d}")) + ";";
                command += EnableDisableTrigger(tableName, triggerName, "enable");

                _command.CreateCommand(command);

                for (int i = 0; i < columnsModel.Names.Count; i++)
                    _command.AddParameter("@" + columnsModel.Names[i], columnsModel.Values[i]);

                var linesAffected = _command.ExecuteNonQuery();
                _command.CloseConneciton();
                if (linesAffected > 0)
                    return Result.BuildSucess();

                return Result.BuildError($"Erro ao atualizar registro. " +
                        $"Guid: {objectGuid} " +
                        $"Json: {objectJson}");
            }
            catch (Exception error)
            {
                return Result.BuildError($"Erro ao atualizar registro. " +
                          $"Guid: {objectGuid} " +
                          $"Json: {objectJson} ", error);
            }
        }

        public Result DeleteObject(string tableName, Guid objectGuid)
        {
            try
            {
                var triggerName = GetTriggerName(tableName);

                var command = EnableDisableTrigger(tableName, triggerName, "disable");
                command += $"delete from {tableName} where guid = @guid;";
                command += EnableDisableTrigger(tableName, triggerName, "enable");

                _command.CreateCommand(command);
                _command.AddParameter("@guid", objectGuid);

                var linesAffected = _command.ExecuteNonQuery();

                _command.CloseConneciton();
                if (linesAffected > 0)
                    return Result.BuildSucess();

                return Result.BuildError($"Erro ao atualizar registro. " +
                        $"TableName: {tableName} " +
                        $"Guid: {objectGuid}");
            }
            catch (Exception error)
            {
                return Result.BuildError($"Erro ao deletar registro. " +
                            $"TableName: {tableName} " +
                            $"Guid: {objectGuid} ", error);
            }
        }

        public SyncHistoric GetBySyncGuid(Guid syncGuid)
        {
            return Set.FirstOrDefault(d => d.SyncGuid == syncGuid);
        }

        public string GetTriggerName(string tableName)
        {
            var strCommand = @"SELECT tr.name AS TriggerName FROM sys.triggers tr
                            INNER JOIN sys.tables t ON t.object_id = tr.parent_id
                            WHERE t.name = @name; ";

            _command.CreateCommand(strCommand);
            _command.AddParameter("@name", tableName);

            var reader = _command.ExecuteReader();
            while (reader.Read())
            {
                var triggerName = reader[0];

                _command.CloseConneciton();
                return triggerName.ToString();
            }

            return "";
        }

        private string EnableDisableTrigger(string tableName, string triggerName, string function)
            => $@"alter table {tableName} {function} trigger {triggerName};";
    }
}
