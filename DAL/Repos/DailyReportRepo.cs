using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
