using System;
using System.Collections.Generic;
using System.Text;
using DAL.Rows;

namespace DAL.Repos
{
    public class CheckReportRepo : Repo<CheckReport>
    {
        public static CheckReportRepo Instance { get; private set; } = new CheckReportRepo();

        private CheckReportRepo()
        {
            TableName = "baocaochamcong";
            PKColsName = new string[]
            {
                "NgayBaoCao"
            };
        }

        public CheckReport FindCurrentDateReport()
        {
            return FindByID(new object[] { DateTime.Now.Date });
        }
    }
}
