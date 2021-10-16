using MySql.Data.MySqlClient;
using System;

namespace DAL
{
    public class DatabaseConnector
    {
        #region Private fields

        private static MySqlConnection _connection;
        private static string _connectionString = "Server=127.0.0.1;Database=EzHRM;Uid=root;Pwd=password123";

        private static int _connectionAttemptCount = 0;

        #endregion

        #region Public prop

        public static MySqlConnection Connection
        {
            get
            {
                if (_connection == null)
                    _connection = new MySqlConnection(_connectionString);

                return _connection;
            }
            private set
            {
                _connection = value;
            }
        }

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
