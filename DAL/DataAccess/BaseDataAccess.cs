using MySql.Data.MySqlClient;
using SqlKata.Compilers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using SqlKata;
using System.Threading.Tasks;
using SqlKata.Execution;

namespace DAL.DataAccess
{
    /// <summary>
    /// YOU THINK I"M NOT GONNA COPY PASTE WHAT YOU HAVE DONE LAST SEMESTER?????? >:V
    /// </summary>
    /// <typeparam name="Model"> The model class </typeparam>
    /// <typeparam name="IDType"> The type if the id of the class </typeparam>
    public abstract class BaseDataAccess<Model, IDType> where Model : DAL.Models.BaseModel
    {
        protected MySqlCommand cmd;

        protected abstract string _tableName { get; }

        public BaseDataAccess()
        {
            cmd = new MySqlCommand
            {
                Connection = DatabaseConnector.Connection
            };
            db = new QueryFactory(DatabaseConnector.Connection, new MySqlCompiler());
        }

        public int GetNextAutoID()
        {
            string q = $@"SELECT `AUTO_INCREMENT`
                        FROM INFORMATION_SCHEMA.TABLES
                        WHERE TABLE_SCHEMA = '{_databaseName}'
                        AND TABLE_NAME = '{_tableName}'";
            cmd.CommandText = q;
            cmd.Parameters.Clear();

            DatabaseConnector.OpenConnection();
            try
            {
                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    int nextID = System.Convert.ToInt32(result);
                    return nextID;
                }
                return -1;
            }
            catch { throw; }
            finally { DatabaseConnector.CloseConnection(); }
        }

        public abstract void Create(Model inpcObject);
        public abstract ObservableCollection<Model> GetAll();
        public abstract void Delete(IDType key);
        public virtual void Update(Model inpcObject)
        {
            throw new System.NotImplementedException();
        }

        protected QueryFactory db;

        private const string _databaseName = "academicsavingservice";
    }
}
