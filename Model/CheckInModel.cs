using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.ObjectModel;
using DAL.Repos;
using DAL.Rows;

namespace Model
{
    public class CheckInModel : CheckIn
    {
        public static ObservableCollection<CheckInModel> LoadAll()
        {
            var checkInList = CheckInRepo.Instance.GetAll();
            var resultList = new ObservableCollection<CheckInModel>();

            foreach (var checkIn in checkInList)
            {
                resultList.Add(new CheckInModel(checkIn));
            }
            return resultList;
        }

        public static ObservableCollection<CheckInModel> LoadTodayOnly()
        {
            var checkInList = CheckInRepo.Instance.GetAll();
            var resultList = new ObservableCollection<CheckInModel>();

            foreach (var checkIn in checkInList)
            {
                if (checkIn.ThoiGianVaoLam.Date == DateTime.Now.Date
                    || (checkIn.ThoiGianVaoLam.Date == DateTime.Now.Date.AddDays(-1)
                        && checkIn.ThoiGianTanLam == null))
                    resultList.Add(new CheckInModel(checkIn));
            }
            return resultList;
        }

        public CheckInModel(CheckIn checkIn) : base(checkIn) { }
        public CheckInModel(CheckInModel checkInModel) : base(checkInModel) { }
    }
}
