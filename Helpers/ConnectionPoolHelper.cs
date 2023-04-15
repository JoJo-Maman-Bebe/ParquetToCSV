using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace FileReaderAPI.Helpers
{
    public class ConnectionPoolHelper
    {
        private readonly string _connectionString;
        private readonly int _maxConnections;
        private readonly Queue<SqlConnection> _connectionQueue;

        public ConnectionPoolHelper(string connectionString, int maxConnections)
        {
            _connectionString = connectionString;
            _maxConnections = maxConnections;
            _connectionQueue = new Queue<SqlConnection>();
        }

        public SqlConnection GetConnection()
        {
            lock (_connectionQueue)
            {
                if (_connectionQueue.Count > 0)
                {
                    SqlConnection connection = _connectionQueue.Dequeue();
                    if (IsConnectionValid(connection))
                    {
                        return connection;
                    }
                    else
                    {
                        connection.Close();
                        return GetConnection();
                    }
                }
                else if (_connectionQueue.Count < _maxConnections)
                {
                    SqlConnection connection = new SqlConnection(_connectionString);
                    connection.Open();
                    _connectionQueue.Enqueue(connection);
                    return connection;
                }
                else
                {
                    throw new Exception("Connection pool exceeded maximum connections.");
                }
            }
        }

        public void ReleaseConnection(SqlConnection connection)
        {
            lock (_connectionQueue)
            {
                if (IsConnectionValid(connection) && _connectionQueue.Count < _maxConnections)
                {
                    _connectionQueue.Enqueue(connection);
                }
                else
                {
                    connection.Close();
                }
            }
        }

        private bool IsConnectionValid(SqlConnection connection)
        {
            try
            {
                using (var command = new SqlCommand("SELECT 1", connection))
                {
                    return connection.State == System.Data.ConnectionState.Open &&
                           command.ExecuteScalar() != null;
                }
            }
            catch
            {
                return false;
            }
        }

    }
}