using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Helper;

namespace ViewModel
{
    public class RoleManagementViewModel : Navigation.CRUDViewModelBase
    {
        public override string ViewName => "Positions";
        private static RoleManagementViewModel _instance = null;
        public static RoleManagementViewModel Instance { get => _instance; }

        public ObservableCollection<CalculateSalaryModel> CalculateSalary { get; set; }
        public ObservableCollection<AccountGroupModel> AccountGroups { get; set; }
        public ObservableCollection<RoleModel> Roles { get; set; }

        private RoleModel _selectedRole = null;
        private RoleModel _currentRole = null;
        private CalculateSalaryModel _selectedCalculateSalary = null;
        private AccountGroupModel _selectedAccountGroup = null;

        public RoleModel SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                CurrentRole = value;
            }
        }

        public RoleModel CurrentRole
        {
            get => _currentRole;
            set
            {
                _currentRole = value;

                foreach (CalculateSalaryModel cs in CalculateSalary)
                {
                    if (_currentRole != null && cs.Ten == _currentRole.CachTinhLuong)
                    {
                        SelectedCalculateSalary = cs;
                    }
                }

                foreach (AccountGroupModel ag in AccountGroups)
                {
                    if (_currentRole != null && ag.TenNhomTaiKhoan == _currentRole.NhomTaiKhoan)
                    {
                        SelectedAccountGroup = ag;
                    }
                }

                StartUpdateCommand.RaiseCanExecuteChangeEvent();
            }
        }

        public AccountGroupModel SelectedAccountGroup
        {
            get => _selectedAccountGroup;
            set
            {
                _selectedAccountGroup = value;
                if (_currentRole != null && _selectedRole != null)
                    _currentRole.NhomTaiKhoan = _selectedAccountGroup.TenNhomTaiKhoan;
            }
        }

        public CalculateSalaryModel SelectedCalculateSalary
        {
            get => _selectedCalculateSalary;
            set
            {
                _selectedCalculateSalary = value;
                if (_currentRole != null && _selectedRole != null)
                    _currentRole.CachTinhLuong = _selectedCalculateSalary.Ten;
            }
        }

        #region Function
        public override void ExecuteAdd(object param)
        {
            CurrentRole = new RoleModel();

            SelectedAccountGroup = null;
            SelectedCalculateSalary = null;

            base.ExecuteAdd(param);
        }
        public override void ExecuteUpdate(object param)
        {
            base.ExecuteUpdate(param);
            CurrentRole = new RoleModel(SelectedRole);
        }

        public override void ExecuteConfirmAdd(object param)
        {
            var result = CurrentRole.Add();

            if (result == "")
            {
                base.ExecuteConfirmAdd(param);
                Roles.Add(CurrentRole);
                SelectedRole = CurrentRole;
                SetCurrentModelBack();
            }
            else
            {
                ErrorString = result;
                HaveError = true;
            }
        }

        public override void ExecuteConfirmUpdate(object param)
        {
            var result = CurrentRole.Save();

            if (result == "")
            {
                base.ExecuteConfirmUpdate(param);
                var found = Roles.FirstOrDefault(x => x.TenChucVu == SelectedRole.TenChucVu);
                Roles[Roles.IndexOf(found)] = CurrentRole;
                SetCurrentModelBack();
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
            SetCurrentModelBack();
        }
        public override void ExecuteCancleUpdate(object param)
        {
            base.ExecuteCancleUpdate(param);
            SetCurrentModelBack();
        }

        public override bool CanExecuteAddStart(object param)
        {
            return base.CanExecuteAddStart(param);
        }

        public override bool CanExecuteUpdateStart(object param)
        {
            return base.CanExecuteUpdateStart(param) && CurrentRole != null;
        }

        public bool CanExecuteSelectProfile(object param)
        {
            return IsInCRUDMode;
        }

        private RelayCommand<object> _deleteCommand;
        public RelayCommand<object> DeleteCommand => _deleteCommand ?? (_deleteCommand = new RelayCommand<object>(Delete, null));

        private void Delete(object obj)
        {
            if (CurrentRole.DaXoa == 0)
                CurrentRole.DaXoa = 1;
            else
                CurrentRole.DaXoa = 0;
        }
        #endregion

        public RoleManagementViewModel()
        {
            _instance = this;
            AccountGroups = AccountGroupModel.LoadAll();
            Roles = RoleModel.LoadAll();
            CalculateSalary = CalculateSalaryModel.LoadAll();
        }

        private void SetCurrentModelBack()
        {
            CurrentRole = SelectedRole;
            StartUpdateCommand.RaiseCanExecuteChangeEvent();
        }
    }
}
