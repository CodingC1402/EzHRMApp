using DAL.Repos;
using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Model
{
    public class HolidayModel : Holiday
    {
        public static ObservableCollection<HolidayModel> LoadAll()
        {
            var rows = HolidayRepo.Instance.GetAll();
            var result = new ObservableCollection<HolidayModel>();

            foreach (var row in rows)
            {
                result.Add(new HolidayModel(row));
            }

            return result;
        }

        public HolidayModel() { }
        public HolidayModel(Holiday holiday) : base(holiday) { }
        public HolidayModel(HolidayModel holidayModel) : base(holidayModel) { }
    }
}
