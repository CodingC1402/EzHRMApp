using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Model;

namespace ViewModel
{
    public class CheckInManagementViewModel : Navigation.ViewModelBase
    {
        public override string ViewName => "Check-Ins";

        public ObservableCollection<CheckInModel> CheckIns { get; set; }

        public CheckInManagementViewModel()
        {
            CheckIns = CheckInModel.LoadAll();
        }
    }
}
