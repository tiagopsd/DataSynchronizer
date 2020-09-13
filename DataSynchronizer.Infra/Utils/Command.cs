using DataSynchronizer.Domain.Entities;
using DataSynchronizer.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataSynchronizer.Infra.Utils
{
    public class Command
    {
        private SqlConnection _sqlConnection;
        private SqlCommand _sqlCommand;
        public Command(Context context)
        {
            _sqlConnection = (SqlConnection)context.Database.GetDbConnection();
        }

        private void OpenConnection()
        {
            if (_sqlConnection.State != System.Data.ConnectionState.Open)
                _sqlConnection.Open();
        }

        public void CloseConneciton()
        {
            if (_sqlConnection.State != System.Data.ConnectionState.Closed)
                _sqlConnection.Close();
        }

        public SqlDataReader ExecuteReader()
        {
            OpenConnection();
            var result = _sqlCommand.ExecuteReader();
            return result;
        }

        public void CreateCommand(string command)
        {
            _sqlCommand = _sqlConnection.CreateCommand();
            _sqlCommand.Connection = _sqlConnection;
            _sqlCommand.CommandText = command;
        }

        public void AddParameter(string parameterName, object value)
        {
            _sqlCommand.Parameters.AddWithValue(parameterName, value);
        }

        public int ExecuteNonQuery()
        {
            OpenConnection();
            var result = _sqlCommand.ExecuteNonQuery();
            return result;
        }
    }
}
