using System;
using System.Collections.ObjectModel;
using Model;
using ViewModel.Helper;
using ViewModel.Structs;

namespace ViewModel
{
    public class StaffsViewModel : Navigation.CRUDViewModelBase
    {
        public override string ViewName => "Staffs";
        public enum ReportTimeSpanEnum
        {
            InWeek,
            InMonth,
            InYear
        }

        public ObservableCollection<EmployeeModel> Employees { get; set; }
        public ObservableCollection<DepartmentModel> AvailableDepartment { get; set; }
        public ObservableCollection<RoleModel> AvailableRole { get; set; }

        public EmployeeReportModel Report { get; set; }
        public ReportTimeSpanEnum ReportTimeSpan { get; set; }
        public ReportTimeSpanEnum[] AvailableTimeSpan { get; } ={
            ReportTimeSpanEnum.InMonth,
            ReportTimeSpanEnum.InWeek,
            ReportTimeSpanEnum.InYear
        };

        private int _selectedDepartmentIndex = 0;
        public int SelectedDepartmentIndex
        {
            get => _selectedDepartmentIndex;
            set
            {
                _selectedDepartmentIndex = value;
                if (_currentEmployee != null && _selectedDepartmentIndex >= 0)
                {
                    _currentEmployee.PhongBan = AvailableDepartment[_selectedDepartmentIndex].TenPhong;
                }
            }
        }

        // Use index to make it easier in search for department
        public int SelectedEmployeeIndex { get; set; }
        private EmployeeModel _selectedEmployee = null;
        private EmployeeModel _currentEmployee = null;
        private RoleModel _selectedRole = null;

        public PenaltyModel ViewingPenalty { get; set; }
        public ObservableCollection<PenaltyTypeModel> PenaltyTypeModels { get; set; }
        private int _penaltyTypeIndex = 0;
        public int PenaltyTypeIndex
        {
            get => _penaltyTypeIndex;
            set
            {
                _penaltyTypeIndex = value;
                if (ViewingPenalty != null && PenaltyTypeModels != null && PenaltyTypeModels.Count > 0)
                {
                    ViewingPenalty.TenViPham = PenaltyTypeModels[_penaltyTypeIndex].TenViPham;
                }
            }
        }

        public EmployeeModel CurrentEmployee
        {
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
                AddPenaltyCommand.RaiseCanExecuteChangeEvent();

                if (_selectedEmployee == null)
                {
                    return;
                }

                UpdateReport();
                DAL.Rows.ProfilePicture profile = value.GetProfilePicture();
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
                {
                    _currentEmployee.ChucVu = _selectedRole.TenChucVu;
                }
            }
        }

        public bool IsViewingReport { get; set; }

        #region Commands
        private RelayCommand<object> _selectProfileCommand;
        public RelayCommand<object> SelectProfileCommand => _selectProfileCommand ??= new RelayCommand<object>(ExecuteSelectProfile, CanExecuteSelectProfile);

        private RelayCommand<object> _addPenaltyCommand;
        public RelayCommand<object> AddPenaltyCommand => _addPenaltyCommand ??= new RelayCommand<object>(ExecuteAddPenalty, CanExecuteAddPenalty);

        private RelayCommand<object> _viewChangeCommand;
        public RelayCommand<object> ViewChangeCommand => _viewChangeCommand ??= new RelayCommand<object>(ExecuteChangeView, CanExecuteChangeView);

        private RelayCommand<object> _confirmAddPenaltyCommand;
        public RelayCommand<object> ConfirmAddPenaltyCommand => _confirmAddPenaltyCommand ??= new RelayCommand<object>(ExecuteAddPenaltyConfirm, CanExecuteAddPenaltyConfirm);

        private RelayCommand<object> _cancelAddPenaltyCommand;
        public RelayCommand<object> CancelAddPenaltyCommand => _cancelAddPenaltyCommand ??= new RelayCommand<object>(ExecuteAddPenaltyCancel, CanExecuteAddPenaltyCancel);

        #endregion
        #region Functions
        private void UpdateReport()
        {
            DateTime endTime = DateTime.Today;
            DateTime startTime = DateTime.Today;
            switch (ReportTimeSpan)
            {
                case ReportTimeSpanEnum.InWeek:
                    startTime = startTime.AddDays(-7);
                    break;
                case ReportTimeSpanEnum.InMonth:
                    startTime = startTime.AddMonths(-1);
                    break;
                case ReportTimeSpanEnum.InYear:
                    startTime = startTime.AddYears(-1);
                    break;
                default:
                    break;
            }
            Report = EmployeeReportModel.Compile(_selectedEmployee, startTime, endTime);
        }

        public void ExecuteSelectProfile(object param)
        {
            if (param == null)
            {
                return;
            }

            ProfilePicture = new Image(param as Image);
            DAL.Rows.ProfilePicture employeeProfile = CurrentEmployee.GetProfilePicture();

            employeeProfile.Image = ProfilePicture.ImageBytes;
            employeeProfile.Width = ProfilePicture.Width;
            employeeProfile.Type = ProfilePicture.FileType;
        }

        public override void ExecuteAdd(object param)
        {
            base.ExecuteAdd(param);
            EmployeeModel newEmployee = new EmployeeModel
            {
                ID = EmployeeModel.GetNextEmployeeID(),

                NgaySinh = DateTime.Today,
                NgayVaoLam = DateTime.Today
            };

            SelectedDepartmentIndex = -1;
            SelectedRole = null;

            CurrentEmployee = newEmployee;
            ProfilePicture = new Image(CurrentEmployee.GetProfilePicture());

            SelectProfileCommand.RaiseCanExecuteChangeEvent();
            ViewChangeCommand.RaiseCanExecuteChangeEvent();
        }
        public override void ExecuteUpdate(object param)
        {
            base.ExecuteUpdate(param);
            CurrentEmployee = new EmployeeModel(SelectedEmployee);

            SelectProfileCommand.RaiseCanExecuteChangeEvent();
            ViewChangeCommand.RaiseCanExecuteChangeEvent();
        }

        public override void ExecuteConfirmAdd(object param)
        {
            string result = CurrentEmployee.Save(true);
            if (result == "")
            {
                Employees.Add(CurrentEmployee);
                SelectedEmployeeIndex = Employees.Count - 1;
                base.ExecuteConfirmAdd(param);
            }
            else
            {
                ErrorString = result;
                HaveError = true;
            }
        }
        public override void ExecuteConfirmUpdate(object param)
        {
            string result = CurrentEmployee.Save(false);
            if (result == "")
            {
                Employees[SelectedEmployeeIndex] = CurrentEmployee;
                SelectedEmployee = CurrentEmployee;
                if (SelectedEmployee.ID == LoginInfo.EmployeeID)
                {
                    LoginInfo.UpdateEmployee();
                    LoggedInViewModel.Instance.UpdateToEmployee();
                }
                base.ExecuteConfirmUpdate(param);
            }
            else
            {
                ErrorString = result;
                HaveError = true;
            }
        }

        public override bool CanExecuteAddStart(object param)
        {
            return base.CanExecuteAddStart(param);
        }

        public override bool CanExecuteUpdateStart(object param)
        {
            return base.CanExecuteUpdateStart(param) && CurrentEmployee != null;
        }

        #region Penalty
        public void ExecuteAddPenaltyConfirm(object param)
        {
            string result = ViewingPenalty.Add();
            if (result == "")
            {
                IsInAddMode = false;

                Report.NumberOfPenalty++;
                switch (ViewingPenalty.TenViPham)
                {
                    case PenaltyModel.Absence:
                        Report.BeingAbsence++;
                        break;
                    case PenaltyModel.BeingLate:
                        Report.BeingLate++;
                        break;
                }

                ViewingPenalty = null;
                RaiseCanRecheck();
            }
            else
            {
                ErrorString = result;
                HaveError = true;
            }
        }
        public void ExecuteAddPenaltyCancel(object param)
        {
            IsInAddMode = false;

            ViewingPenalty = null;
            RaiseCanRecheck();
        }

        public bool CanExecuteAddPenaltyConfirm(object param)
        {
            return IsInCRUDMode && IsViewingReport;
        }
        public bool CanExecuteAddPenaltyCancel(object param)
        {
            return IsInCRUDMode && IsViewingReport;
        }

        public void ExecuteAddPenalty(object param)
        {
            IsInAddMode = true;

            PenaltyTypeIndex = 0;
            PenaltyTypeModels = PenaltyTypeModel.LoadAll();

            ViewingPenalty = new PenaltyModel
            {
                Ngay = DateTime.Today,
                IDNhanVien = SelectedEmployee.ID
            };

            RaiseCanRecheck();
        }
        public bool CanExecuteAddPenalty(object param)
        {
            return _selectedEmployee != null && IsInNormalMode && IsViewingReport;
        }
        #endregion

        protected override void OnModeChangeBack()
        {
            SetCurrentModelBack();
            RaiseCanRecheck();
        }

        protected void RaiseCanRecheck()
        {
            AddPenaltyCommand.RaiseCanExecuteChangeEvent();
            ViewChangeCommand.RaiseCanExecuteChangeEvent();
            CancelAddPenaltyCommand.RaiseCanExecuteChangeEvent();
            ConfirmAddPenaltyCommand.RaiseCanExecuteChangeEvent();
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

        private void ExecuteChangeView(object param)
        {
            IsViewingReport = !IsViewingReport;
            RaiseCanRecheck();
        }

        private bool CanExecuteChangeView(object param)
        {
            return !IsInCRUDMode;
        }
        #endregion

        protected override void PopUpMessageClosed(object sender, EventArgs e)
        {
            base.PopUpMessageClosed(sender, e);
        }
    }
}
