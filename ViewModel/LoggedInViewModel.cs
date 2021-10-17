using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Helper;
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

        protected RelayCommand<object> _logoutCommand = null;
        public RelayCommand<object> LogOutCommand => _logoutCommand ?? (_logoutCommand = new RelayCommand<object>(param => ExecuteLogout()));
        protected void ExecuteLogout()
        {
            LoginInfo.Logout();
            var vm = MainViewModel.GetInstance();
            vm.CurrentViewModel = vm.ToLogin.ViewModel;
        }

        public LoggedInViewModel()
        {
            if (__instance != null)
            {
                throw new Exception("There is another MainViewModel");
            }
            __instance = this;

            

            ToHomeView = new NavigationCommand<HomeViewModel>(new HomeViewModel(), this, 0);
            ViewModels.Add(ToHomeView.ViewModel);

            ToDashboard = new NavigationCommand<DashboardViewModel>(new DashboardViewModel(), this, 0);
            ViewModels.Add(ToHomeView.ViewModel);

            ToStaffsManagementView = new NavigationCommand<StaffsManagementViewModel>(new StaffsManagementViewModel(), this, 0);
            ViewModels.Add(ToHomeView.ViewModel);

            CurrentViewModel = ToHomeView.ViewModel;
        }

        public NavigationCommand<HomeViewModel> ToHomeView { get; set; }
        public NavigationCommand<DashboardViewModel> ToDashboard { get; set; }
        public NavigationCommand<StaffsManagementViewModel> ToStaffsManagementView { get; set; }
    }
}
