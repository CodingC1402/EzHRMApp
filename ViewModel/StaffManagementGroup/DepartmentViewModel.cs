using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Helper;
using ViewModel.Structs;

namespace ViewModel
{
    public class DepartmentViewModel : Navigation.CRUDViewModelBase
    {
        public override string ViewName => "Departments";
        public ObservableCollection<DepartmentModel> Departments { get; set; }

        private DepartmentModel _selectedDepartment = null;
        private DepartmentModel _currentDepartment = null;

        public DepartmentModel CurrentDepartment
        {
            get => _currentDepartment;
            set
            {
                _currentDepartment = value;
                StartUpdateCommand.RaiseCanExecuteChangeEvent();
            }
        }

        public DepartmentModel SelectedDepartment
        {
            get => _selectedDepartment;
            set
            {
                _selectedDepartment = value;
                CurrentDepartment = value;
            }
        }

        bool _isPopupOpened = false;
        public bool IsPopupOpened
        {
            get => _isPopupOpened;
            set
            {
                _isPopupOpened = value;
                UpdatePopup();
            }
        }

        public bool OnlyMessage { get; set; }
        public string Message { get; set; }
        public Image ProfilePicture { get; set; }

        private EmployeeModel _manager = null;
        public EmployeeModel Manager
        {
            get => _manager;
            set
            {
                _manager = value;

                var profile = value != null ? value.GetProfilePicture() : null;

                ProfilePicture = profile != null ? new Image(profile) : null;
            }
        }

        #region Function
        public override void ExecuteAdd(object param)
        {
            var newDepartment = new DepartmentModel();
            newDepartment.NgayThanhLap = DateTime.Now;

            CurrentDepartment = newDepartment;

            base.ExecuteAdd(param);
        }
        public override void ExecuteUpdate(object param)
        {
            base.ExecuteUpdate(param);
            CurrentDepartment = new DepartmentModel(SelectedDepartment);
        }

        public override void ExecuteConfirmAdd(object param)
        {
            var result = CurrentDepartment.Add();

            if (result == "")
            {
                base.ExecuteConfirmAdd(param);
                Departments.Add(CurrentDepartment);
                SelectedDepartment = CurrentDepartment;
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
            var result = CurrentDepartment.Save();

            if (result == "")
            {
                base.ExecuteConfirmUpdate(param);
                var found = Departments.FirstOrDefault(x => x.TenPhong == SelectedDepartment.TenPhong);
                Departments[Departments.IndexOf(found)] = CurrentDepartment;
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
            return base.CanExecuteUpdateStart(param) && CurrentDepartment != null;
        }

        public bool CanExecuteSelectProfile(object param)
        {
            return IsInCRUDMode;
        }
        #endregion

        public DepartmentViewModel()
        {
            Departments = DepartmentModel.LoadAll();
            OnlyMessage = true;
            Message = "No manager here!";
        }

        private void SetCurrentModelBack()
        {
            CurrentDepartment = SelectedDepartment;
            StartUpdateCommand.RaiseCanExecuteChangeEvent();
        }

        private void UpdatePopup()
        {
            if (CurrentDepartment != null)
            {
                Manager = EmployeeModel.GetEmployeeByID(CurrentDepartment.TruongPhong);

                if (Manager == null)
                {
                    if (CurrentDepartment.TruongPhong == null)
                    {
                        Message = "No manager here!";
                    }
                    else
                    {
                        Message = "Unknown employee ID: " + CurrentDepartment.TruongPhong;
                    }

                    OnlyMessage = true;
                }
                else
                {
                    OnlyMessage = false;
                    Message = "";
                }
            }
        }
    }
}
