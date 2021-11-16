using DAL.Repos;
using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class DailyReportModel : DailyReport
    {
        public DailyReportModel() { }
        public DailyReportModel(DailyReport report) : base(report) { }

        public static DailyReportModel GetReportOfDate(DateTime date)
        {
            var report = DailyReportRepo.Instance.FindByID(new object[] { date });
            if (report == null)
                return null;
            else
            {
                return new DailyReportModel(report);
            }
        }
    }
}
