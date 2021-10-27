using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Model;
using System.Windows;
using ViewModel.Helper;
using Microsoft.Win32;
using System.Drawing;
using ViewModel.Structs;

namespace ViewModel
{
    public class StaffsViewModel : Navigation.CRUDViewModelBase
    {

        public ObservableCollection<EmployeeModel> Employees { get; set; }
        public ObservableCollection<string> AvailableDepartment { get; set; } = new ObservableCollection<string>
        {
            "Cleaner",
            "Coder",
            "Shithead"
        };

        private EmployeeModel _selectedEmployee = null;
        private EmployeeModel _currentEmployee = null;

        public EmployeeModel CurrentEmployee {
            get => _currentEmployee;
            set
            {
                _currentEmployee = value;
                StartUpdateCommand.RaiseCanExecuteChangeEvent();
            }
        }
        public Image ProfilePicture { get; set; }

        public EmployeeModel SelectedEmployee {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                var profile = value.GetProfilePicture();

                ProfilePicture = profile != null ? new Image(profile) : null;
                CurrentEmployee = value;
            }
        }

        private RelayCommand<object> _selectProfileCommand;
        public RelayCommand<object> SelectProfileCommand => _selectProfileCommand ?? (_selectProfileCommand = new RelayCommand<object>(ExecuteSelectProfile, CanExecuteSelectProfile));

        public void ExecuteSelectProfile(object param)
        {
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
            base.ExecuteConfirmAdd(param);
            CurrentEmployee.Save();
            SetCurrentModelBack();
        }
        public override void ExecuteConfirmUpdate(object param)
        {
            base.ExecuteConfirmUpdate(param);
            CurrentEmployee.Save();
            SetCurrentModelBack();
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
