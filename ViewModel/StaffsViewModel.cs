using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModel.Helper;

namespace ViewModel
{
    public class StaffsViewModel : Navigation.ViewModelBase
    {
        public enum Mode
        {
            Insert,
            Edit
        }

        public override string ViewName => "Staffs";

        public Mode CurrentMode { get; set; }

        [PropertyChanged.OnChangedMethod(nameof(OnSelectedChange))]
        public Staffs.Employee SelectedEmployee { get; set; }

        public void OnSelectedChange()
        {
            CurrentEmployee = new Staffs.Employee(SelectedEmployee);
        }

        public Staffs.Employee CurrentEmployee { get; set; }
        public ObservableCollection<Staffs.Employee> EmployeeList { get; set; }

        protected RelayCommand<object> _addStaffRelayCommand;
        public ICommand AddStaffCommand => _addStaffRelayCommand ?? (_addStaffRelayCommand = new RelayCommand<object>(ExecuteAddStaff, CanExecuteAddStaff));

        protected RelayCommand<object> _changeModeRelayCommand;
        public ICommand ChangeModeCommand => _changeModeRelayCommand ?? (_changeModeRelayCommand = new RelayCommand<object>(ExecuteChangeMode, CanExecuteChangeMode));

        public StaffsViewModel()
        {
            CurrentMode = Mode.Edit;
            EmployeeList = Staffs.GetList();

            if (EmployeeList.Count > 0)
                SelectedEmployee = EmployeeList[0];
        }

        private bool CanExecuteAddStaff(object obj)
        {
            return true;
        }

        private void ExecuteAddStaff(object obj)
        {
            if (Staffs.CheckStaffInfo(CurrentEmployee))
            {
                return;
            }

            Staffs.AddStaff(CurrentEmployee);
            EmployeeList.Add(CurrentEmployee);
            EmployeeList.Count();
        }

        private bool CanExecuteChangeMode(object obj)
        {
            return true;
        }

        private void ExecuteChangeMode(object obj)
        {
            
            switch (CurrentMode)
            {
                case Mode.Insert:
                    CurrentMode = Mode.Edit;
                    break;
                case Mode.Edit:
                    CurrentMode = Mode.Insert;
                    break;
                default:
                    break;
            }
        }
    }
}
