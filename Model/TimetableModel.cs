using System;
using System.Collections.Generic;
using System.Text;
using DAL.Rows;
using DAL.Repos;

namespace Model
{
    public class TimetableModel : Timetable
    {
        public static TimetableModel CurrentTimetableModel => new TimetableModel(TimetableRepo.Instance.FindLatestTimetable());

        public TimetableModel() { }
        public TimetableModel(Timetable t) : base(t) { }
    }
}
