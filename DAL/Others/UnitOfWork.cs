using DAL.Rows;
using SqlKata;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Others
{
    public class UnitOfWork : IDisposable
    {
        List<Query> _queries = new List<Query>();
        public Dictionary<string, List<Row>> _repos = new Dictionary<string, List<Row>>();

        public List<Query> Queries { get => _queries; }
        public Dictionary<string, List<Row>> Repos { get => _repos; }

        public virtual bool Complete()
        {
            try
            {
                DatabaseConnector.Connection.Open();
                using (var scope = DatabaseConnector.Connection.BeginTransaction())
                {
                    try
                    {
                        var db = DatabaseConnector.Database;
                        foreach (Query query in _queries)
                        {
                            db.Execute(query);
                        }
                    }
                    catch
                    {
                        scope.Rollback();
                        return false;
                    }

                    scope.Commit();
                    return true;
                }
            }
            finally
            {
                DatabaseConnector.Connection.Close();
            }
        }

        public void Dispose()
        {
            _queries.Clear();
            _repos.Clear();
        }
    }
}
