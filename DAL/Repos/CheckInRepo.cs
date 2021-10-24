using DAL.Rows;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos
{
    public class CheckInRepo : Repo<CheckIn>
    {
        public static CheckInRepo Instance { get; private set; } = new CheckInRepo();
        private CheckInRepo()
        {
            TableName = "CHAMCONG";
            PKColsName = new string[]
            {
                "ThoiGianVaoLam"
            };
        }

        public CheckIn FindNewestCheckIn(string employeeID)
        {
            try
            {
                var db = DatabaseConnector.Database;
                return db.Query(TableName).Where(nameof(CheckIn.IDNhanVien), employeeID).OrderByDesc(nameof(CheckIn.ThoiGianVaoLam)).Limit(1).First<CheckIn>();
            }
            catch
            { return null; }
        }
    }
}
