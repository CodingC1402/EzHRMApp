﻿using System;
using System.Collections.ObjectModel;
using Model;
using ViewModel.Helper;
using ViewModel.Structs;

namespace ViewModel
{
    public class StaffsViewModel : Navigation.CRUDViewModelBase
    {
        public ObservableCollection<EmployeeModel> Employees { get; set; }
        public ObservableCollection<DepartmentModel> AvailableDepartment { get; set; }

        private ObservableCollection<RoleModel> _availableRoles;
        public ObservableCollection<RoleModel> AvailableRole {
            get => _availableRoles;
            set => _availableRoles = value;
        }

        private int _selectedDepartmentIndex = 0;
        public int SelectedDepartmentIndex
        {
            get => _selectedDepartmentIndex;
            set
            {
                _selectedDepartmentIndex = value;
                if (_currentEmployee != null && _selectedDepartmentIndex >= 0)
                    _currentEmployee.PhongBan = AvailableDepartment[_selectedDepartmentIndex].TenPhong;
            }
        }

        public int SelectedEmployeeIndex {get; set;}
        private EmployeeModel _selectedEmployee = null;
        private EmployeeModel _currentEmployee = null;
        private RoleModel _selectedRole = null;

        public EmployeeModel CurrentEmployee {
            get => _currentEmployee;
            set
            {
                _currentEmployee = value;

                AvailableRole = new ObservableCollection<RoleModel>();
                foreach (RoleModel role in RoleManagementViewModel.Instance.Roles)
                {
                    if (role.DaXoa == 0 || (_currentEmployee != null && _currentEmployee.NgayThoiViec != null))
                    {
                        AvailableRole.Add(role);
                        if (_currentEmployee != null && role.TenChucVu == _currentEmployee.ChucVu)
                        {
                            SelectedRole = role;
                        }
                    }
                }
                SelectedDepartmentIndex = _currentEmployee != null ? DepartmentModel.GetIndex(_currentEmployee, AvailableDepartment) : -1;
                StartUpdateCommand.RaiseCanExecuteChangeEvent();
            }
        }
        public Image ProfilePicture { get; set; }

        public EmployeeModel SelectedEmployee {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                if (_selectedEmployee == null)
                    return;

                var profile = value.GetProfilePicture();
                ProfilePicture = profile != null ? new Image(profile) : null;
                CurrentEmployee = value;
            }
        }

        public RoleModel SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                if (_currentEmployee != null && _selectedRole != null)
                _currentEmployee.ChucVu = _selectedRole.TenChucVu;
            }
        }

        private RelayCommand<object> _selectProfileCommand;
        public RelayCommand<object> SelectProfileCommand => _selectProfileCommand ?? (_selectProfileCommand = new RelayCommand<object>(ExecuteSelectProfile, CanExecuteSelectProfile));

        public void ExecuteSelectProfile(object param)
        {
            if (param == null)
                return;

            ProfilePicture = new Image(param as Image);
            var employeeProfile = CurrentEmployee.GetProfilePicture();

            employeeProfile.Image = ProfilePicture.ImageBytes;
            employeeProfile.Width = ProfilePicture.Width;
            employeeProfile.Type = ProfilePicture.FileType;
        }

        public override void ExecuteAdd(object param)
        {
            base.ExecuteAdd(param);
            var newEmployee = new EmployeeModel();
            newEmployee.ID = EmployeeModel.GetNextEmployeeID();

            newEmployee.NgaySinh = DateTime.Today;
            newEmployee.NgayVaoLam = DateTime.Today;

            SelectedDepartmentIndex = -1;
            SelectedRole = null;

            CurrentEmployee = newEmployee;
            ProfilePicture = new Image(CurrentEmployee.GetProfilePicture());
            SelectProfileCommand.RaiseCanExecuteChangeEvent();
        }
        public override void ExecuteUpdate(object param)
        {
            base.ExecuteUpdate(param);
            CurrentEmployee = new EmployeeModel(SelectedEmployee);
            SelectProfileCommand.RaiseCanExecuteChangeEvent();
        }

        public override void ExecuteConfirmAdd(object param)
        {
            var result = CurrentEmployee.Save(true);
            if (result == "")
            {
                base.ExecuteConfirmAdd(param);
                Employees.Add(CurrentEmployee);
                SelectedEmployeeIndex = Employees.Count - 1;
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
            var result = CurrentEmployee.Save(false);
            if (result == "")
            {
                base.ExecuteConfirmUpdate(param);
                Employees[SelectedEmployeeIndex] = CurrentEmployee;
                SelectedEmployee = CurrentEmployee;
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
            return base.CanExecuteUpdateStart(param) && CurrentEmployee != null;
        }

        public bool CanExecuteSelectProfile(object param)
        {
            return IsInCRUDMode;
        }

        public StaffsViewModel()
        {
            Employees = EmployeeModel.LoadAll();
            AvailableDepartment = DepartmentModel.LoadAll();
            SelectedDepartmentIndex = -1;
        }

        private void SetCurrentModelBack()
        {
            CurrentEmployee = SelectedEmployee;
            ProfilePicture = CurrentEmployee != null ? new Image(CurrentEmployee.ProfilePicture) : null;
            SelectProfileCommand.RaiseCanExecuteChangeEvent();
            StartUpdateCommand.RaiseCanExecuteChangeEvent();
        }
    }
}
