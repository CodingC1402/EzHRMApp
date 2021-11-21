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

        public PayrollTypesViewModel()
        {
            Instance = this;
        }
    }
}
