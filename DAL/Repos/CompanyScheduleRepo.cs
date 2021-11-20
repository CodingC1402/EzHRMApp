using DAL.Rows;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos
{
    public class CompanyScheduleRepo :Repo<CompanySchedule>
    {
        public static CompanyScheduleRepo Instance { get; private set; } = new CompanyScheduleRepo();

        private CompanyScheduleRepo()
        {
            TableName = "THOIGIANBIEUTUAN";
            PKColsName = new string[]
            {
                "ThoiDiemTao"
            };
        }

        public CompanySchedule GetLatestVariables()
        {
            return DatabaseConnector.Database.Query(TableName).OrderByDesc(PKColsName).Limit(1).First<CompanySchedule>();
        }
    }
}
