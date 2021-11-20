using System;
using System.Collections.ObjectModel;
using System.Linq;
using Model;
using DAL.Rows;
using ViewModel.Helper;
using ViewModel.Structs;
using CsvHelper;
using System.IO;
using System.Globalization;
using CsvHelper.Configuration;

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

        private ObservableCollection<RoleModel> _availableRoles;
        public ObservableCollection<RoleModel> AvailableRole {
            get => _availableRoles;
            set => _availableRoles = value;
        }

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
                    _currentEmployee.PhongBan = AvailableDepartment[_selectedDepartmentIndex].TenPhong;
            }
        }

        // Use index to make it easier in search for department
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
                AddPenaltyCommand.RaiseCanExecuteChangeEvent();

                if (_selectedEmployee == null)
                    return;
                UpdateReport();
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

        #region Commands
        private RelayCommand<object> _selectProfileCommand;
        public RelayCommand<object> SelectProfileCommand => _selectProfileCommand ?? (_selectProfileCommand = new RelayCommand<object>(ExecuteSelectProfile, CanExecuteSelectProfile));

        private RelayCommand<object> _addPenaltyCommand;
        public RelayCommand<object> AddPenaltyCommand => _addPenaltyCommand ?? (_addPenaltyCommand = new RelayCommand<object>(ExecuteAddPenalty, CanExecuteAddPenalty));

        private RelayCommand<string> _importCommand;
        public RelayCommand<string> ImportCommand => _importCommand ??= new RelayCommand<string>(ExecuteImport);
        #endregion

        #region Functions
        private void UpdateReport()
        {
            var endTime = DateTime.Today;
            var startTime = DateTime.Today;
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
            }
            Report = EmployeeReportModel.Compile(_selectedEmployee, startTime, endTime);
        }

        public void ExecuteImport(string path)
        {
            if (path == null)
                return;

            CultureInfo cinfo = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            cinfo.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            cinfo.DateTimeFormat.LongDatePattern = "dd/MM/yyyy";
            cinfo.DateTimeFormat.FullDateTimePattern = "dd/MM/yyyy";

            using (var streamReader = new StreamReader(path))
            using (var reader = new CsvReader(streamReader, cinfo))
            {
                try
                {
                    var employees = reader.GetRecords<Employee>();
                    foreach (var e in employees)
                    {
                        var existing = Employees.FirstOrDefault(eModel => eModel.ID == e.ID 
                        || eModel.EmailCaNhan == e.EmailCaNhan 
                        || eModel.EmailVanPhong == e.EmailVanPhong);

                        if (existing != null)
                        {
                            if (existing.ID == e.ID)
                                ErrorString = $"Employee with ID {existing.ID} already exists";
                            else if (existing.EmailCaNhan == e.EmailCaNhan)
                                ErrorString = $"Employee with personal email {existing.EmailCaNhan} already exists";
                            else
                                ErrorString = $"Employee with work email {existing.EmailVanPhong} already exists";

                            HaveError = true;
                            return;
                        }

                        var newEmployee = new EmployeeModel(e);
                        string result = newEmployee.Save(true);
                        if (result == "")
                            Employees.Add(newEmployee);
                        else
                            throw new Exception(result);
                    }
                }
                catch (Exception ex)
                {
                    ErrorString = ex.Message;
                    HaveError = true;
                }
            }
        }

        public void ExecuteAddPenalty(object param)
        {

        }
        public bool CanExecuteAddPenalty(object param)
        {
            return _selectedEmployee != null;
        }

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
            var result = CurrentEmployee.Save(false);
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

        protected override void OnModeChangeBack()
        {
            SetCurrentModelBack();
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
        #endregion

        protected override void PopUpMessageClosed(object sender, EventArgs e)
        {
            base.PopUpMessageClosed(sender, e);
        }
    }
}
