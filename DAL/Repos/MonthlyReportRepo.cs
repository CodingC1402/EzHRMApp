using System;
using System.Collections.Generic;
using System.Text;
using DAL.Rows;

namespace DAL.Repos
{
    public class MonthlyReportRepo : Repo<MonthlyReport>
    {
        public static MonthlyReportRepo Instance { get; set; } = new MonthlyReportRepo();

        private MonthlyReportRepo()
        {
            TableName = "baocaonhansu";
            PKColsName = new string[]
            {
                "Thang",
                "Nam"
            };
        }

        public MonthlyReport FindByDate(DateTime date)
        {
            return FindByID(new object[] { date.Month, date.Year });
        }

        public MonthlyReport FindCurrentMonthlyReport()
        {
            return FindByID(new object[] { DateTime.Now.Month, DateTime.Now.Year });
        }
    }
}
