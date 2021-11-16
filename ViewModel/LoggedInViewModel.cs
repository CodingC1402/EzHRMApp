using DAL;
using Model;
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
            ViewModels.Add(ToDashboard.ViewModel);

            ToScheduleManagementView = new NavigationCommand<ScheduleManagementViewModel>(new ScheduleManagementViewModel(), this, 1);
            ViewModels.Add(ToScheduleManagementView.ViewModel);

            ToStaffView = new NavigationCommand<StaffsViewModel>(new StaffsViewModel(), this, 0);
            ViewModels.Add(ToStaffView.ViewModel);

            ToDepartmentView = new NavigationCommand<DepartmentViewModel>(new DepartmentViewModel(), this, 0);
            ViewModels.Add(ToDepartmentView.ViewModel);

            ToRoleManagementView = new NavigationCommand<RoleManagementViewModel>(new RoleManagementViewModel(), this, 0);
            ViewModels.Add(ToRoleManagementView.ViewModel);

            ToPayRollManagementView = new NavigationCommand<PayRollManagementViewModel>(new PayRollManagementViewModel(), this, 0);
            ViewModels.Add(ToPayRollManagementView.ViewModel);

            ToReportsView = new NavigationCommand<ReportsViewModel>(new ReportsViewModel(), this, 0);
            ViewModels.Add(ToReportsView.ViewModel);

            CurrentViewModel = ToHomeView.ViewModel;
        }

        public NavigationCommand<HomeViewModel> ToHomeView { get; set; }
        public NavigationCommand<DashboardViewModel> ToDashboard { get; set; }
        public NavigationCommand<ScheduleManagementViewModel> ToScheduleManagementView { get; set; }
        public NavigationCommand<StaffsViewModel> ToStaffView { get; set; }
        public NavigationCommand<DepartmentViewModel> ToDepartmentView { get; set; }
        public NavigationCommand<RoleManagementViewModel> ToRoleManagementView { get; set; }
        public NavigationCommand<PayRollManagementViewModel> ToPayRollManagementView { get; set; }
        public NavigationCommand<ReportsViewModel> ToReportsView { get; set; }
    }
}
