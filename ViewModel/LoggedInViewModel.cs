using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Navigation;

namespace ViewModel
{
    public class LoggedInViewModel : Navigation.NavigationViewModel
    {
        public override string ViewName => "Main";

        private static LoggedInViewModel __instance = null;
        public static LoggedInViewModel GetInstance()
        {
            return __instance;
        }

        public LoggedInViewModel()
        {
            if (__instance != null)
            {
                throw new Exception("There is another MainViewModel");
            }
            __instance = this;

            ToHomeView = new NavigationCommand<HomeViewModel>(new HomeViewModel(), this);
            ViewModels.Add(ToHomeView.ViewModel);

            ToDashboard = new NavigationCommand<DashboardViewModel>(new DashboardViewModel(), this);
            ViewModels.Add(ToHomeView.ViewModel);

            ToStaffsManagementView = new NavigationCommand<StaffsManagementViewModel>(new StaffsManagementViewModel(), this);
            ViewModels.Add(ToHomeView.ViewModel);

            CurrentViewModel = ToHomeView.ViewModel;
        }

        public NavigationCommand<HomeViewModel> ToHomeView { get; set; }
        public NavigationCommand<DashboardViewModel> ToDashboard { get; set; }
        public NavigationCommand<StaffsManagementViewModel> ToStaffsManagementView { get; set; }
    }
}
