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

        private bool _isWaitingForDeleteConfirmation = false;
        public static PayrollTypesViewModel Instance { get; private set; }

        public ObservableCollection<PaymentMethodModel> PaymentMethods { get; set; }
        public PaymentMethodModel CurrentMethod { get; set; }
        private PaymentMethodModel _selectedMethod = null;
        public PaymentMethodModel SelectedMethod
        {
            get => _selectedMethod;
            set
            {
                _selectedMethod = value;
                StartUpdateCommand.RaiseCanExecuteChangeEvent();
                DeleteCommand.RaiseCanExecuteChangeEvent();
                CurrentMethod = value;
            }
        }

        private RelayCommand<object> _deleteCommand;
        public RelayCommand<object> DeleteCommand => _deleteCommand ?? (_deleteCommand = new RelayCommand<object>(Delete, CanDelete));

        private bool CanDelete(object param)
        {
            return SelectedMethod != null && !SelectedMethod.IsSpecialType;
        }

        private void Delete(object param)
        {
            _isWaitingForDeleteConfirmation = true;
            PopUpMessage.Instance.Message = "Are you sure you want to delete this Payroll Type?";
            ShowConfirmation = true;
        }

        protected override void PopUpMessageClosed(object sender, EventArgs e)
        {
            base.PopUpMessageClosed(sender, e);

            if (_isWaitingForDeleteConfirmation)
            {
                _isWaitingForDeleteConfirmation = false;
                if (MessageResult == PopUpMessage.Result.Ok)
                {
                    string result = SelectedMethod.Delete();
                    if (result == "")
                    {
                        PaymentMethods.Remove(SelectedMethod);
                        SelectedMethod = null;
                    }
                    else
                    {
                        ErrorString = result;
                        HaveError = true;
                    }
                }
            }
        }

        public override void ExecuteAdd(object param)
        {
            base.ExecuteAdd(param);
            CurrentMethod = new PaymentMethodModel();
            CurrentMethod.LanTraLuongCuoi = DateTime.Now;
            CurrentMethod.NgayTinhLuongThangNay = DateTime.Now;
        }

        public override void ExecuteUpdate(object param)
        {
            base.ExecuteUpdate(param);
            CurrentMethod = new PaymentMethodModel(SelectedMethod);
        }

        public override void ExecuteConfirmAdd(object param)
        {
            string result = CurrentMethod.Add();
            if (result == "")
            {
                PaymentMethods.Add(CurrentMethod);
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
            string result = CurrentMethod.Save();
            if (result == "")
            {
                SelectedMethod = CurrentMethod;
                var found = PaymentMethods.FirstOrDefault(x => x.Ten == SelectedMethod.Ten);
                PaymentMethods[PaymentMethods.IndexOf(found)] = SelectedMethod;
                base.ExecuteConfirmUpdate(param);
            }
            else
            {
                ErrorString = result;
                HaveError = true;
            }
        }

        public override bool CanExecuteUpdateStart(object param)
        {
            return base.CanExecuteUpdateStart(param) && SelectedMethod != null;
        }

        protected override void OnModeChangeBack()
        {
            base.OnModeChangeBack();
            CurrentMethod = SelectedMethod;
        }

        public override void OnGetTo()
        {
            base.OnGetTo();
            PaymentMethods = PaymentMethodModel.LoadAll();
            if (PaymentMethods != null && PaymentMethods.Count > 0)
            {
                SelectedMethod = PaymentMethods[0];
            }
        }

        public PayrollTypesViewModel()
        {
            Instance = this;
            PaymentMethods = PaymentMethodModel.LoadAll();
        }
    }
}
