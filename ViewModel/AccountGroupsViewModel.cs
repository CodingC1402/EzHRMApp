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
            }
        }
        public AccountGroupModel SelectedAccountGroup
        {
            get => _selectedAG;
            set
            {
                _selectedAG = value;
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
            CurrentAccountGroup = new AccountGroupModel("", 0);
            base.ExecuteAdd(param);
        }

        public override void ExecuteUpdate(object param)
        {
            base.ExecuteUpdate(param);
            CurrentAccountGroup = new AccountGroupModel(SelectedAccountGroup);
        }

        public override void ExecuteConfirmAdd(object param)
        {
            var result = CurrentAccountGroup.Add();

            if (result == "")
            {
                base.ExecuteConfirmAdd(param);
                AccountGroups.Add(CurrentAccountGroup);
                SelectedAccountGroup = CurrentAccountGroup;
                StartUpdateCommand.RaiseCanExecuteChangeEvent();
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
                CurrentAccountGroup = SelectedAccountGroup;
                StartUpdateCommand.RaiseCanExecuteChangeEvent();
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
            return base.CanExecuteUpdateStart(param) && CurrentAccountGroup != null;
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

        public RelayCommand<object> DashboardToggleCommand => _dashboardCommand ??=
            new RelayCommand<object>(obj => {
                CurrentAccountGroup.DashboardViewPermission = !CurrentAccountGroup.DashboardViewPermission;
                System.Diagnostics.Debug.WriteLine($"Dashboard: {CurrentAccountGroup.DashboardViewPermission}");
            } , null);

        public RelayCommand<object> CheckInToggleCommand => _checkInCommand ??=
            new RelayCommand<object>(obj => CurrentAccountGroup.CheckInViewPermission = !CurrentAccountGroup.CheckInViewPermission, null);

        public RelayCommand<object> ScheduleToggleCommand => _scheduleCommand ??=
            new RelayCommand<object>(obj => CurrentAccountGroup.ScheduleViewPermission = !CurrentAccountGroup.ScheduleViewPermission, null);

        public RelayCommand<object> StaffToggleCommand => _staffCommand ??=
            new RelayCommand<object>(obj => CurrentAccountGroup.StaffViewPermission = !CurrentAccountGroup.StaffViewPermission, null);

        public RelayCommand<object> DepartmentToggleCommand => _departmentCommand ??=
            new RelayCommand<object>(obj => CurrentAccountGroup.DepartmentsViewPermission = !CurrentAccountGroup.DepartmentsViewPermission, null);

        public RelayCommand<object> PositionToggleCommand => _positionCommand ??=
            new RelayCommand<object>(obj => CurrentAccountGroup.PositionsViewPermission = !CurrentAccountGroup.PositionsViewPermission, null);

        public RelayCommand<object> PayrollToggleCommand => _payrollCommand ??=
            new RelayCommand<object>(obj => CurrentAccountGroup.PayrollViewPermission = !CurrentAccountGroup.PayrollViewPermission, null);

        public RelayCommand<object> AccountGroupToggleCommand => _accountGroupCommand ??=
            new RelayCommand<object>(obj => CurrentAccountGroup.AccountGroupsViewPermission = !CurrentAccountGroup.AccountGroupsViewPermission, null);

        public RelayCommand<object> ReportToggleCommand => _reportCommand ??=
            new RelayCommand<object>(obj => CurrentAccountGroup.ReportsViewPermission = !CurrentAccountGroup.ReportsViewPermission, null);

        private bool canExecuteTogglePermissions(object param)
        {
            return IsInCRUDMode;
        }
        #endregion
    }
}
