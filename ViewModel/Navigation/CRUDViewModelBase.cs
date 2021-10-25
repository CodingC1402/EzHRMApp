using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModel.Helper;

namespace ViewModel.Navigation
{
    public class CRUDViewModelBase : ViewModelBase
    {
        public bool IsInUpdateMode { get; set; }
        public bool IsInAddMode { get; set; }

        private RelayCommand<object> _startUpdateCommand;
        public ICommand StartUpdateCommand => _startUpdateCommand ?? (_startUpdateCommand = new RelayCommand<object>(ExecuteUpdate));
        private RelayCommand<object> _startAddCommand;
        public ICommand StartAddCommand => _startAddCommand ?? (_startAddCommand = new RelayCommand<object>(ExecuteAdd));

        private RelayCommand<object> _confirmUpdateCommand;
        public ICommand ConfirmUpdateCommand => _confirmUpdateCommand ?? (_confirmUpdateCommand = new RelayCommand<object>(ExecuteConfirmUpdate, CanExecuteUpdateConfirm));
        private RelayCommand<object> _confirmAddCommand;
        public ICommand ConfirmAddCommand => _confirmAddCommand ?? (_confirmAddCommand = new RelayCommand<object>(ExecuteConfirmAdd, CanExecuteAddConfirm));

        private RelayCommand<object> _cancleUpdateCommand;
        public ICommand CancleUpdateCommand => _cancleUpdateCommand ?? (_cancleUpdateCommand = new RelayCommand<object>(ExecuteCancleUpdate, CanExecuteUpdateConfirm));
        private RelayCommand<object> _cancleAddCommand;
        public ICommand CancleAddCommand => _cancleAddCommand ?? (_cancleAddCommand = new RelayCommand<object>(ExecuteCancleAdd, CanExecuteAddConfirm));

        public virtual void ExecuteAdd(object param)
        {
            IsInAddMode = true;
        }
        public virtual void ExecuteUpdate(object param)
        {
            IsInUpdateMode = true;
        }

        public virtual void ExecuteConfirmAdd(object param)
        {
            IsInAddMode = false;
        }
        public virtual void ExecuteConfirmUpdate(object param)
        {
            IsInUpdateMode = false;
        }

        public virtual void ExecuteCancleAdd(object param)
        {
            IsInAddMode = false;
        }
        public virtual void ExecuteCancleUpdate(object param)
        {
            IsInUpdateMode = false;
        }

        public virtual bool CanExecuteAddConfirm(object param)
        {
            return IsInAddMode;
        }
        public virtual bool CanExecuteUpdateConfirm(object param)
        {
            return IsInUpdateMode;
        }
    }
}
