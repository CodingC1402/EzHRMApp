using System;
using System.Collections.Generic;
using System.Text;
using ViewModel.Helper;
using ViewModel.Navigation;

namespace ViewModel
{
    public class PayRollManagementViewModel : NavigationViewModel
    {
        public override string ViewName => "Payroll";

        public PayRollManagementViewModel()
        {
            ToEmployeePayroll = new NavigationCommand<EmployeePayrollManagementViewModel>(new EmployeePayrollManagementViewModel(), this, 0);
            ViewModels.Add(ToEmployeePayroll.ViewModel);

            ToPayrollTypes = new NavigationCommand<PayrollTypesViewModel>(new PayrollTypesViewModel(), this, 0);
            ViewModels.Add(ToPayrollTypes.ViewModel);

            CurrentViewModel = ToEmployeePayroll.ViewModel;
        }

        public NavigationCommand<EmployeePayrollManagementViewModel> ToEmployeePayroll;
        public NavigationCommand<PayrollTypesViewModel> ToPayrollTypes;
    }
}
