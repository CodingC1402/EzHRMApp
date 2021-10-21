using DAL.Rows;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DAL.Repos
{
    public class Repo<TRow, IDType> where TRow : Row
    { 
        //Important
        public virtual string TableName { get => "Repo"; }
        public virtual string IDColName { get => "ColName"; }

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
        public TRow FindByID(IDType id)
        {
            try
            {
                var db = DatabaseConnector.Database;
                return db.Query(TableName).Where(IDColName, id).First<TRow>();
            }
            catch
            {
                return null;
            }

        }
        public IEnumerable<TRow> FindBy(IEnumerable<KeyValuePair<string, object>> clauses, bool first)
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

        public bool Add(TRow newRow)
        {
            try
            {
                var db = DatabaseConnector.Database;
                db.Query(TableName).Insert(newRow);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool AddRange(IEnumerable<TRow> newRows)
        {
            try
            {
                var db = DatabaseConnector.Database;
                foreach (TRow row in newRows)
                {
                    db.Query(TableName).Insert(row);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public virtual bool Update(IDType id, TRow updatedRow)
        {
            try
            {
                var db = DatabaseConnector.Database;
                db.Query(TableName).Where(IDColName, id).Update(updatedRow);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Update(IEnumerable<KeyValuePair<string, object>> clauses, TRow updatedRow)
        {
            try
            {
                var db = DatabaseConnector.Database;
                db.Query(TableName).Where(clauses).Update(updatedRow);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateRange(IEnumerable<KeyValuePair<IEnumerable<KeyValuePair<string, object>>, TRow>> updates)
        {
            try
            {
                var db = DatabaseConnector.Database;
                foreach (var update in updates)
                {
                    db.Query(TableName).Where(update.Key).Update(update.Value);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Remove(IDType id)
        {
            try
            {
                var db = DatabaseConnector.Database;
                db.Query(TableName).Where(IDColName, id).Delete();
                return true;
            }
            catch { return false; }
        }

        public bool Remove(IEnumerable<KeyValuePair<string, object>> clauses)
        {
            try
            {
                var db = DatabaseConnector.Database;
                db.Query(TableName).Where(clauses).Delete();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool RemoveRange(IEnumerable<IEnumerable<KeyValuePair<string, object>>> range)
        {
            try
            {
                var db = DatabaseConnector.Database;
                foreach (var clauses in range)
                {
                    db.Query(TableName).Where(clauses).Delete();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
