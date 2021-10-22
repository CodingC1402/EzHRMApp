using MySql.Data.MySqlClient;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;

namespace DAL
{
    public class DatabaseConnector
    {
        #region Private fields

        private static string _connectionString = "Server=127.0.0.1;Database=EzHRM;Uid=root;Pwd=password123";

        private static MySqlConnection _connection = new MySqlConnection(_connectionString);
        private static MySqlCompiler _compiler = new MySqlCompiler();
        private static QueryFactory _database = new QueryFactory(_connection, _compiler);

        private static int _connectionAttemptCount = 0;

        #endregion

        #region Public prop

        public static MySqlConnection Connection { get => _connection; private set => _connection = value; }
        public static QueryFactory Database { get => _database; private set => _database = value; }

        #endregion

        public static void OpenConnection()
        {
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }
            _connectionAttemptCount++;
        }

        public static void CloseConnection()
        {
            if (_connection.State != System.Data.ConnectionState.Closed)
            {
                _connectionAttemptCount--;
                if (_connectionAttemptCount == 0)
                {
                    _connection.Close();
                }
            }
        }
    }
}
