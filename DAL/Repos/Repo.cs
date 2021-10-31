using DAL.Others;
using DAL.Rows;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DAL.Repos
{
    public class Repo<TRow> where TRow : Row
    {
        //Important
        public virtual string TableName { get; protected set; }
        public virtual string[] PKColsName { get; protected set; }

        public IEnumerable<TRow> GetAll()
        {
            try
            {
                var db = DatabaseConnector.Database;
                return db.Query(TableName).Get<TRow>();
            }
            catch
            {
                return null;
            }
        }
        public TRow FindByID(object[] pkKeys)
        {
            try
            {
                var db = DatabaseConnector.Database;

                var q = db.Query(TableName);
                q = AddPKClause(pkKeys, q);
                return q.First<TRow>();
            }
            catch
            {
                return null;
            }

        }
        public IEnumerable<TRow> FindBy(string colName, object value)
        {
            try
            {
                var db = DatabaseConnector.Database;

                var q = db.Query(TableName);
                return q.Where(colName, value).Get<TRow>();
            }
            catch { return null; }
        }
        public IEnumerable<TRow> FindBy(KeyValuePair<string, object>[] clauses, bool first)
        {
            try
            {
                var db = DatabaseConnector.Database;
                var q = db.Query(TableName).Where(clauses);
                if (first)
                {
                    var list = new List<TRow>();
                    list.Add(q.First<TRow>());
                    return list;
                }
                else
                {
                    return q.Get<TRow>();
                }
            }
            catch { return null; }
        }

        public bool Add(TRow newRow, UnitOfWork uow)
        {
            var db = DatabaseConnector.Database;
            uow.Queries.Add(db.Query(TableName).AsInsert(newRow));
            return true;
        }
        public bool AddRange(IEnumerable<TRow> newRows, UnitOfWork uow)
        {
            var db = DatabaseConnector.Database;
            foreach (TRow row in newRows)
            {
                uow.Queries.Add(db.Query(TableName).AsInsert(row));
            }
            return true;
        }

        public virtual bool Update(object[] pkKeys, TRow updatedRow, UnitOfWork uow)
        {
            var db = DatabaseConnector.Database;

            var q = db.Query(TableName);
            q = AddPKClause(pkKeys, q);
            
            uow.Queries.Add(q.AsUpdate(updatedRow));
            return true;
        }
        public bool Update(IEnumerable<KeyValuePair<string, object>> clauses, TRow updatedRow, UnitOfWork uow)
        {
            var db = DatabaseConnector.Database;
            uow.Queries.Add(db.Query(TableName).Where(clauses).AsUpdate(updatedRow));
            return true;
        }
        public bool UpdateRange(IEnumerable<KeyValuePair<IEnumerable<KeyValuePair<string, object>>, TRow>> updates, UnitOfWork uow)
        {
            var db = DatabaseConnector.Database;
            foreach (var update in updates)
            {
                uow.Queries.Add(db.Query(TableName).Where(update.Key).AsUpdate(update.Value));
            }
            return true;
        }

        public bool Remove(object[] pkKeys, UnitOfWork uow)
        {
            var db = DatabaseConnector.Database;
            var q = AddPKClause(pkKeys, db.Query(TableName));

            uow.Queries.Add(q.AsDelete());
            return true;
        }

        public bool Remove(KeyValuePair<string, object>[] clauses, UnitOfWork uow)
        {
            var db = DatabaseConnector.Database;
            uow.Queries.Add(db.Query(TableName).Where(clauses).AsDelete());
            return true;
        }
        public bool RemoveRange(IEnumerable<KeyValuePair<string, object>[]> range, UnitOfWork uow)
        {
            var db = DatabaseConnector.Database;
            foreach (var clauses in range)
            {
                uow.Queries.Add(db.Query(TableName).Where(clauses).AsDelete());
            }
            return true;
        }

        public SqlKata.Query AddPKClause(object[] pkKeys, SqlKata.Query query)
        {
            var q = query;
            for (int i = 0; i < PKColsName.Length; i++)
            {
                q = q.Where(PKColsName[i], pkKeys[i]);
            }
            return q;
        }
    }
}
