using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Navigation;

namespace ViewModel
{
    public class StaffsManagementViewModel : Navigation.NavigationViewModel
    {
        public override string ViewName => "Staffs Management";
        private static StaffsManagementViewModel __instance = null;
        public static StaffsManagementViewModel GetInstance()
        {
            return __instance;
        }

        public StaffsManagementViewModel()
        {
            __instance = this;


            ToStaffView = new NavigationCommand<StaffsViewModel>(new StaffsViewModel(), this, 0);
            ViewModels.Add(ToStaffView.ViewModel);

            CurrentViewModel = ToStaffView.ViewModel;
        }


        public NavigationCommand<StaffsViewModel> ToStaffView { get; set; }
    }
}
