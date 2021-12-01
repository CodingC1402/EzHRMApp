using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ViewModel
{
    public class HolidayViewModel : Navigation.CRUDViewModelBase
    {
        public override string ViewName => "Holiday";
        private static HolidayViewModel _instance = null;
        public static HolidayViewModel Instance { get => _instance; }

        public ObservableCollection<HolidayModel> Holidays { get; set; }

        public HolidayViewModel()
        {
            _instance = this;
            Holidays = HolidayModel.LoadAll();
        }
    }
}
