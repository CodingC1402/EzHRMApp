using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ViewModel;
using System.Linq;
using ViewModel.Helper;
using Model;

namespace ViewModel
{
    public class AccountGroupsViewModel : Navigation.CRUDViewModelBase
    {
        public override string ViewName => "Account Groups";

        public ObservableCollection<AccountGroupModel> AccountGroups { get; set; }

        public AccountGroupModel CurrentAccountGroup
        {
            get => _currentAG;
            set
            {
                _currentAG = value;
                StartUpdateCommand.RaiseCanExecuteChangeEvent();
                DeleteCommand.RaiseCanExecuteChangeEvent();
            }
        }
        public AccountGroupModel SelectedAccountGroup
        {
            get => _selectedAG;
            set
            {
                _selectedAG = value;
                if (value != null)
                    CurrentAccountGroup = value;
            }
        }

        private AccountGroupModel _currentAG;
        private AccountGroupModel _selectedAG;

        public AccountGroupsViewModel()
        {
            AccountGroups = AccountGroupModel.LoadAll();
        }

        #region Overriden command methods
        public override void ExecuteAdd(object param)
        {
            base.ExecuteAdd(param);
            CurrentAccountGroup = new AccountGroupModel("", 0, false);
        }

        public override void ExecuteUpdate(object param)
        {
            base.ExecuteUpdate(param);
            CurrentAccountGroup = new AccountGroupModel(SelectedAccountGroup);
        }

        public override void ExecuteConfirmAdd(object param)
        {
            var result = CurrentAccountGroup.Add();

            if (result == "" || result == "group-exists")
            {
                base.ExecuteConfirmAdd(param);
                AccountGroups.Add(CurrentAccountGroup);
                SelectedAccountGroup = CurrentAccountGroup;
                StartUpdateCommand.RaiseCanExecuteChangeEvent();
                DeleteCommand.RaiseCanExecuteChangeEvent();
            }
            else
            {
                ErrorString = result;
                HaveError = true;
            }
        }

        public override void ExecuteConfirmUpdate(object param)
        {
            var result = CurrentAccountGroup.Save();

            if (result == "")
            {
                base.ExecuteConfirmUpdate(param);
                var found = AccountGroups.FirstOrDefault(x => x.TenNhomTaiKhoan == SelectedAccountGroup.TenNhomTaiKhoan);
                AccountGroups[AccountGroups.IndexOf(found)] = CurrentAccountGroup;
                SelectedAccountGroup = CurrentAccountGroup;
                StartUpdateCommand.RaiseCanExecuteChangeEvent();
                DeleteCommand.RaiseCanExecuteChangeEvent();
            }
            else
            {
                ErrorString = result;
                HaveError = true;
            }
        }

        public override void ExecuteCancleAdd(object param)
        {
            base.ExecuteCancleAdd(param);
            CurrentAccountGroup = SelectedAccountGroup;
        }
        public override void ExecuteCancleUpdate(object param)
        {
            base.ExecuteCancleUpdate(param);
            CurrentAccountGroup = SelectedAccountGroup;
        }

        public override bool CanExecuteAddStart(object param)
        {
            return base.CanExecuteAddStart(param);
        }

        public override bool CanExecuteUpdateStart(object param)
        {
            return base.CanExecuteUpdateStart(param) && CurrentAccountGroup != null && !SelectedAccountGroup.IsBossGroup;
        }

        private void ExecuteDelete(object param)
        {
            var accounts = AccountModel.GetAccountGroupsOfActiveEmployees();
            if (accounts.Contains(CurrentAccountGroup.TenNhomTaiKhoan))
            {
                ErrorString = "There is at least 1 active employee with this account group!";
                HaveError = true;
                return;
            }

            CurrentAccountGroup.DaXoa = true;
            var result = CurrentAccountGroup.Save();
            if (result != "")
            {
                ErrorString = result;
                HaveError = true;
            }
            else
            {
                AccountGroups.Remove(CurrentAccountGroup);
                CurrentAccountGroup = null;
            }
        }

        private bool CanExecuteDelete(object param)
        {
            return SelectedAccountGroup != null && !IsInCRUDMode && !SelectedAccountGroup.IsBossGroup;
        }
        #endregion

        #region Toggle commands
        private RelayCommand<object> _dashboardCommand;
        private RelayCommand<object> _checkInCommand;
        private RelayCommand<object> _scheduleCommand;
        private RelayCommand<object> _staffCommand;
        private RelayCommand<object> _departmentCommand;
        private RelayCommand<object> _positionCommand;
        private RelayCommand<object> _payrollCommand;
        private RelayCommand<object> _accountGroupCommand;
        private RelayCommand<object> _reportCommand;
        private RelayCommand<object> _holidayCommand;
        private RelayCommand<object> _deleteCommand;

        public RelayCommand<object> DashboardToggleCommand => _dashboardCommand ??=
            new RelayCommand<object>(obj => {
                CurrentAccountGroup.DashboardViewPermission = !CurrentAccountGroup.DashboardViewPermission;
                System.Diagnostics.Debug.WriteLine($"Dashboard: {CurrentAccountGroup.DashboardViewPermission}");
            } , null);

        public RelayCommand<object> CheckInToggleCommand => _checkInCommand ??=
            new RelayCommand<object>(obj => CurrentAccountGroup.CheckInManagementViewPermission = !CurrentAccountGroup.CheckInManagementViewPermission, null);

        public RelayCommand<object> ScheduleToggleCommand => _scheduleCommand ??=
            new RelayCommand<object>(obj => CurrentAccountGroup.WeeklyScheduleViewPermission = !CurrentAccountGroup.WeeklyScheduleViewPermission, null);

        public RelayCommand<object> StaffToggleCommand => _staffCommand ??=
            new RelayCommand<object>(obj => CurrentAccountGroup.StaffViewPermission = !CurrentAccountGroup.StaffViewPermission, null);

        public RelayCommand<object> DepartmentToggleCommand => _departmentCommand ??=
            new RelayCommand<object>(obj => CurrentAccountGroup.DepartmentsViewPermission = !CurrentAccountGroup.DepartmentsViewPermission, null);

        public RelayCommand<object> PositionToggleCommand => _positionCommand ??=
            new RelayCommand<object>(obj => CurrentAccountGroup.RolesViewPermission = !CurrentAccountGroup.RolesViewPermission, null);

        public RelayCommand<object> PayrollToggleCommand => _payrollCommand ??=
            new RelayCommand<object>(obj => CurrentAccountGroup.EmployeePayrollViewPermission = !CurrentAccountGroup.EmployeePayrollViewPermission, null);

        public RelayCommand<object> AccountGroupToggleCommand => _accountGroupCommand ??=
            new RelayCommand<object>(obj => CurrentAccountGroup.AccountGroupsViewPermission = !CurrentAccountGroup.AccountGroupsViewPermission, null);

        public RelayCommand<object> ReportToggleCommand => _reportCommand ??=
            new RelayCommand<object>(obj => CurrentAccountGroup.ReportsViewPermission = !CurrentAccountGroup.ReportsViewPermission, null);

        public RelayCommand<object> HolidayToggleCommand => _holidayCommand ??=
            new RelayCommand<object>(obj => CurrentAccountGroup.HolidayViewPermission = !CurrentAccountGroup.HolidayViewPermission, null);

        public RelayCommand<object> DeleteCommand => _deleteCommand ??=
            new RelayCommand<object>(ExecuteDelete, CanExecuteDelete);
        #endregion
    }
}
