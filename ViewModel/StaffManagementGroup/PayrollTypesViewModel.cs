using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ViewModel.Helper;
using ViewModel.Navigation;

namespace ViewModel
{
    public class PayrollTypesViewModel : CRUDViewModelBase
    {
        public override string ViewName => "Payment Method";

        public static PayrollTypesViewModel Instance { get; private set; }
        public PaymentMethodModel HourlyMethod { get; set; }
        public PaymentMethodModel MonthlyMethod { get; set; }

        public override void ExecuteCancleUpdate(object param)
        {
            base.ExecuteCancleUpdate(param);
            SetCurrentModelBack();
        }

        public override void ExecuteUpdate(object param)
        {
            base.ExecuteUpdate(param);
        }

        public override void ExecuteConfirmUpdate(object param)
        {
            var result = HourlyMethod.Save();

            if (result != "")
            {
                ErrorString = result;
                HaveError = true;
                return;
            }

            result = MonthlyMethod.Save();

            if (result != "")
            {
                ErrorString = result;
                HaveError = true;
                return;
            }

            base.ExecuteConfirmUpdate(param);
            StartUpdateCommand.RaiseCanExecuteChangeEvent();
        }

        public override bool CanExecuteUpdateStart(object param)
        {
            return base.CanExecuteUpdateStart(param) && HourlyMethod != null && MonthlyMethod != null;
        }

        private void SetCurrentModelBack()
        {
            HourlyMethod = PaymentMethodModel.GetMethodModel("TheoGio");
            MonthlyMethod = PaymentMethodModel.GetMethodModel("TheoThang");
            StartUpdateCommand.RaiseCanExecuteChangeEvent();
        }


        public PayrollTypesViewModel()
        {
            Instance = this;
            HourlyMethod = PaymentMethodModel.GetMethodModel("TheoGio");
            MonthlyMethod = PaymentMethodModel.GetMethodModel("TheoThang");
        }
    }
}
