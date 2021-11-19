using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Text;
using SqlKata.Execution;

namespace DAL.Repos
{
    public class DailyReportRepo : Repo<DailyReport>
    {
        public static DailyReportRepo Instance { get; private set; } = new DailyReportRepo();
        private DailyReportRepo()
        {
            TableName = "BAOCAOCHAMCONG";
            PKColsName = new string[] { "NgayBaoCao" };
        }

        public IEnumerable<DailyReport> GetAllReportInTimeSpan(DateTime start, DateTime end)
        {
            try
            {
                var db = DatabaseConnector.Database;

                var q = db.Query(TableName);
                return q.Where(PKColsName[0], ">=", start).Where(PKColsName[0], "<=", end).Get<DailyReport>();
            }
            catch { return null; }
        }
    }
}
