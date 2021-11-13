using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Structs;

namespace ViewModel
{
    public class DepartmentViewModel : Navigation.CRUDViewModelBase
    {
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

        public override void ExecuteAdd(object param)
        {
            base.ExecuteAdd(param);
            var newDepartment = new DepartmentModel();

            CurrentDepartment = newDepartment;
        }
        public override void ExecuteUpdate(object param)
        {
            base.ExecuteUpdate(param);
            CurrentDepartment = new DepartmentModel(SelectedDepartment);
        }

        public override void ExecuteConfirmAdd(object param)
        {
            base.ExecuteConfirmAdd(param);

            if (CurrentDepartment.Add() == 0)
            {
                Departments.Add(CurrentDepartment);
                SelectedDepartment = CurrentDepartment;
            }

            SetCurrentModelBack();
        }
        public override void ExecuteConfirmUpdate(object param)
        {
            base.ExecuteConfirmUpdate(param);

            if (SelectedDepartment.Update(CurrentDepartment) == 0)
            {
                var found = Departments.FirstOrDefault(x => x.TenPhong == SelectedDepartment.TenPhong);
                Departments[Departments.IndexOf(found)] = CurrentDepartment;
            }

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
            return base.CanExecuteUpdateStart(param) && CurrentDepartment != null;
        }

        public bool CanExecuteSelectProfile(object param)
        {
            return IsInCRUDMode;
        }

        //Test
        public Image ProfilePicture { get; set; }
        private EmployeeModel _manager = null;
        public EmployeeModel Manager
        {
            get => _manager;
            set
            {
                _manager = value;
                var profile = value.GetProfilePicture();

                ProfilePicture = profile != null ? new Image(profile) : null;
            }
        }

        public DepartmentViewModel()
        {
            Departments = DepartmentModel.LoadAll();
            Manager = EmployeeModel.LoadAll()[0];
        }

        private void SetCurrentModelBack()
        {
            CurrentDepartment = SelectedDepartment;
            StartUpdateCommand.RaiseCanExecuteChangeEvent();
        }
    }
}
