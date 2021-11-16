using System;
using System.Collections.Generic;
using System.Text;
using SqlKata.Execution;
using DAL.Rows;

namespace DAL.Repos
{
    public class TimetableRepo : Repo<Timetable>
    {
        public static TimetableRepo Instance { get; private set; } = new TimetableRepo();

        public static Timetable CurrentTimetable { get; private set; }

        public TimetableRepo()
        {
            TableName = "THOIGIANBIEUTUAN";
            PKColsName = new string[]
            {
                "ThoiDiemTao"
            };
            FindLatestTimetable();
        }

        public Timetable FindLatestTimetable()
        {
            var db = DatabaseConnector.Database;
            return CurrentTimetable = db.Query(TableName).OrderByDesc(PKColsName).Limit(1).First<Timetable>();
        }
    }
}
