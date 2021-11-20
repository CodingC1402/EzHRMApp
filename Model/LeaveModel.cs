using DAL.Repos;
using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Model
{
    public class LeaveModel : Leave
    {
        public LeaveModel() { }
        public LeaveModel(Leave leave) : base(leave) { }

        public static ObservableCollection<LeaveModel> GetAll()
        {
            ObservableCollection<LeaveModel> rValue = new ObservableCollection<LeaveModel>();
            var result = LeaveRepo.Instance.GetAll();
            foreach (var leave in result)
            {
                rValue.Add(new LeaveModel(leave));
            }
            return rValue;
        }
    }
}
