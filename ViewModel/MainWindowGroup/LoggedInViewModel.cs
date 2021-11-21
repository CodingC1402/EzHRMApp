using DAL;
using Model;
using Model.Utils;
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
    public class LoggedInViewModel : NavigationViewModel
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

            ToUserInfo = new NavigationCommand<UserInfoViewModel>(new UserInfoViewModel(), this, QuyenHanBitmask.UserInfo);
            ViewModels.Add(ToUserInfo.ViewModel);

            ToWeeklyScheduleView = new NavigationCommand<WeeklyScheduleViewModel>(new WeeklyScheduleViewModel(), this, QuyenHanBitmask.WeeklySchedule);
            ViewModels.Add(ToWeeklyScheduleView.ViewModel);

            ToDashboard = new NavigationCommand<DashboardViewModel>(new DashboardViewModel(), this, QuyenHanBitmask.Dashboard);
            ViewModels.Add(ToDashboard.ViewModel);

            ToStaffView = new NavigationCommand<StaffsViewModel>(new StaffsViewModel(), this, QuyenHanBitmask.Staff);
            ViewModels.Add(ToStaffView.ViewModel);

            ToCheckInManagementView = new NavigationCommand<CheckInManagementViewModel>(new CheckInManagementViewModel(), this, QuyenHanBitmask.CheckInManagement);
            ViewModels.Add(ToCheckInManagementView.ViewModel);

            ToLeavesView = new NavigationCommand<LeavesViewModel>(new LeavesViewModel(), this, QuyenHanBitmask.Leaves);
            ViewModels.Add(ToLeavesView.ViewModel);

            ToPenaltyView = new NavigationCommand<PenaltyViewModel>(new PenaltyViewModel(), this, QuyenHanBitmask.Penalty);
            ViewModels.Add(ToPenaltyView.ViewModel);

            ToEmployeePayrollManagementView = new NavigationCommand<EmployeePayrollManagementViewModel>(new EmployeePayrollManagementViewModel(), this, QuyenHanBitmask.EmployeePayroll);
            ViewModels.Add(ToEmployeePayrollManagementView.ViewModel);

            ToReportsView = new NavigationCommand<ReportsViewModel>(new ReportsViewModel(), this, QuyenHanBitmask.Reports);
            ViewModels.Add(ToReportsView.ViewModel);

            ToRoleManagementView = new NavigationCommand<RoleManagementViewModel>(new RoleManagementViewModel(), this, QuyenHanBitmask.Roles);
            ViewModels.Add(ToRoleManagementView.ViewModel);

            ToDepartmentView = new NavigationCommand<DepartmentViewModel>(new DepartmentViewModel(), this, QuyenHanBitmask.Departments);
            ViewModels.Add(ToDepartmentView.ViewModel);

            ToPayrollTypesView = new NavigationCommand<PayrollTypesViewModel>(new PayrollTypesViewModel(), this, QuyenHanBitmask.PayrollTypes);
            ViewModels.Add(ToPayrollTypesView.ViewModel);

            ToPenaltyTypeView = new NavigationCommand<PenaltyTypeViewModel>(new PenaltyTypeViewModel(), this, QuyenHanBitmask.PenaltyTypes);
            ViewModels.Add(ToPenaltyTypeView.ViewModel);

            ToAccountGroupsView = new NavigationCommand<AccountGroupsViewModel>(new AccountGroupsViewModel(), this, QuyenHanBitmask.AccountGroups);
            ViewModels.Add(ToAccountGroupsView.ViewModel);

            ToScheduleManagementView = new NavigationCommand<ScheduleManagementViewModel>(new ScheduleManagementViewModel(), this, QuyenHanBitmask.ScheduleManagement);
            ViewModels.Add(ToScheduleManagementView.ViewModel);
        }

        public override void OnGetTo()
        {
            UpdateToEmployee();
            ToHomeView.Execute(null);
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
        public NavigationCommand<WeeklyScheduleViewModel> ToWeeklyScheduleView { get; set; }
        public NavigationCommand<StaffsViewModel> ToStaffView { get; set; }
        public NavigationCommand<DepartmentViewModel> ToDepartmentView { get; set; }
        public NavigationCommand<RoleManagementViewModel> ToRoleManagementView { get; set; }
        public NavigationCommand<EmployeePayrollManagementViewModel> ToEmployeePayrollManagementView { get; set; }
        public NavigationCommand<PayrollTypesViewModel> ToPayrollTypesView { get; set; }
        public NavigationCommand<PenaltyViewModel> ToPenaltyView { get; set; }
        public NavigationCommand<ReportsViewModel> ToReportsView { get; set; }
        public NavigationCommand<CheckInManagementViewModel> ToCheckInManagementView { get; set; }
        public NavigationCommand<AccountGroupsViewModel> ToAccountGroupsView { get; set; }
        public NavigationCommand<LeavesViewModel> ToLeavesView { get; set; }
        public NavigationCommand<PenaltyTypeViewModel> ToPenaltyTypeView { get; set; }
    }
}
