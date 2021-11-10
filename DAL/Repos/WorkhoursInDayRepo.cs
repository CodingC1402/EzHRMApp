using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos
{
    public class WorkhoursInDayRepo : Repo<WorkhoursInDay>
    {
        public static WorkhoursInDayRepo Instance { get; private set; } = new WorkhoursInDayRepo();

        public WorkhoursInDayRepo()
        {
            TableName = "sogiolamtrongngay";
            PKColsName = new string[]
            {
                "Ngay",
                "IDNhanVien"
            };
        }
    }
}
