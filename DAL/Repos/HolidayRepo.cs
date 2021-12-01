using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos
{
    public class HolidayRepo : Repo<Holiday>
    {
        public static HolidayRepo Instance { get; private set; } = new HolidayRepo();

        public HolidayRepo()
        {
            TableName = "NGHILE";
            PKColsName = new string[]
            {
                "ID"
            };
        }
    }
}
