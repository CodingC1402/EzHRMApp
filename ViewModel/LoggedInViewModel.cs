using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Helper;
using ViewModel.Navigation;
using ViewModel.Structs;

namespace ViewModel
{
    public class LoggedInViewModel : Navigation.NavigationViewModel
    {
        public override string ViewName => "Main";

        private static LoggedInViewModel __instance = null;
        public static LoggedInViewModel Instance { get => __instance; }
        public static LoggedInViewModel GetInstance()
        {
            return __instance;
        }

        public Image ProfilePicture { get; set; }
        public EmployeeModel EmployeeModel { get; set; }

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

            ToUserInfo = new NavigationCommand<UserInfoViewModel>(new UserInfoViewModel(), this, 0);
            ViewModels.Add(ToUserInfo.ViewModel);

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

            ToCheckInManagementView = new NavigationCommand<CheckInManagementViewModel>(new CheckInManagementViewModel(), this, 0);
            ViewModels.Add(ToCheckInManagementView.ViewModel);

            ToAccountGroupsView = new NavigationCommand<AccountGroupsViewModel>(new AccountGroupsViewModel(), this, 0);
            ViewModels.Add(ToAccountGroupsView.ViewModel);

            CurrentViewModel = ToHomeView.ViewModel;
        }

        public override void OnGetTo()
        {
            UpdateToEmployee();
        }

        public void UpdateToEmployee()
        {
            EmployeeModel = LoginInfo.Employee;
            if (LoginInfo.Employee != null)
            {
                ProfilePicture = new Image(LoginInfo.Employee.GetProfilePicture());
            }
        }

        public NavigationCommand<HomeViewModel> ToHomeView { get; set; }
        public NavigationCommand<UserInfoViewModel> ToUserInfo{ get; set; }
        public NavigationCommand<DashboardViewModel> ToDashboard { get; set; }
        public NavigationCommand<ScheduleManagementViewModel> ToScheduleManagementView { get; set; }
        public NavigationCommand<StaffsViewModel> ToStaffView { get; set; }
        public NavigationCommand<DepartmentViewModel> ToDepartmentView { get; set; }
        public NavigationCommand<RoleManagementViewModel> ToRoleManagementView { get; set; }
        public NavigationCommand<PayRollManagementViewModel> ToPayRollManagementView { get; set; }
        public NavigationCommand<ReportsViewModel> ToReportsView { get; set; }
        public NavigationCommand<CheckInManagementViewModel> ToCheckInManagementView { get; set; }
        public NavigationCommand<AccountGroupsViewModel> ToAccountGroupsView { get; set; }
    }
}
