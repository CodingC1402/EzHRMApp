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
        private bool _isInUpdateMode = false;
        private bool _isInAddMode = false;

        public bool HaveError {
            get => PopUpMessage.Instance.IsOpened;
            set
            {
                if (value)
                {
                    PopUpMessage.Instance.PopUpMessageClosed += PopUpMessageClosed;
                    PopUpMessage.Instance.Title = "Error!";
                    PopUpMessage.Instance.ButtonStyle = PopUpMessage.ButtonStyleEnum.ConfirmButton;
                }
                PopUpMessage.Instance.IsOpened = value;
            }
        }
        public string ErrorString {
            get => PopUpMessage.Instance.Message;
            set
            {
                PopUpMessage.Instance.Message = value;
            }
        }
        public PopUpMessage.Result MessageResult
        {
            get => PopUpMessage.Instance.MessageResult;
        }

        public bool IsInNormalMode
        {
            get => !(_isInAddMode || _isInUpdateMode);
        }
        public bool IsInCRUDMode
        {
            get => _isInAddMode || _isInUpdateMode;
        }
        public bool IsInNormalAndUpdateMode
        {
            get => _isInUpdateMode || IsInNormalMode;
        }

        public bool IsInUpdateMode {
            get => _isInUpdateMode;
            set
            {
                if (_isInUpdateMode == value)
                    return;

                _isInUpdateMode = value;
                ConfirmUpdateCommand.RaiseCanExecuteChangeEvent();
                CancleUpdateCommand.RaiseCanExecuteChangeEvent();
                RaisePropertyChanged(nameof(IsInNormalMode));
                RaisePropertyChanged(nameof(IsInCRUDMode));
            }
        }
        public bool IsInAddMode {
            get => _isInAddMode;
            set
            {
                if (value == _isInAddMode)
                    return;

                _isInAddMode = value;
                ConfirmAddCommand.RaiseCanExecuteChangeEvent();
                CancleAddCommand.RaiseCanExecuteChangeEvent();
                RaisePropertyChanged(nameof(IsInNormalMode));
                RaisePropertyChanged(nameof(IsInCRUDMode));
            }
        }

        private RelayCommand<object> _startUpdateCommand;
        public RelayCommand<object> StartUpdateCommand => _startUpdateCommand ?? (_startUpdateCommand = new RelayCommand<object>(ExecuteUpdate, CanExecuteUpdateStart));
        private RelayCommand<object> _startAddCommand;
        public RelayCommand<object> StartAddCommand => _startAddCommand ?? (_startAddCommand = new RelayCommand<object>(ExecuteAdd, CanExecuteAddStart));

        private RelayCommand<object> _confirmUpdateCommand;
        public RelayCommand<object> ConfirmUpdateCommand => _confirmUpdateCommand ?? (_confirmUpdateCommand = new RelayCommand<object>(ExecuteConfirmUpdate, CanExecuteUpdateConfirm));
        private RelayCommand<object> _confirmAddCommand;
        public RelayCommand<object> ConfirmAddCommand => _confirmAddCommand ?? (_confirmAddCommand = new RelayCommand<object>(ExecuteConfirmAdd, CanExecuteAddConfirm));

        private RelayCommand<object> _cancleUpdateCommand;
        public RelayCommand<object> CancleUpdateCommand => _cancleUpdateCommand ?? (_cancleUpdateCommand = new RelayCommand<object>(ExecuteCancleUpdate, CanExecuteUpdateConfirm));
        private RelayCommand<object> _cancleAddCommand;
        public RelayCommand<object> CancleAddCommand => _cancleAddCommand ?? (_cancleAddCommand = new RelayCommand<object>(ExecuteCancleAdd, CanExecuteAddConfirm));

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

        public virtual bool CanExecuteAddStart(object param)
        {
            return !IsInAddMode;
        }

        public virtual bool CanExecuteUpdateStart(object param)
        {
            return !IsInUpdateMode;
        }

        public virtual bool CanExecuteAddConfirm(object param)
        {
            return IsInAddMode;
        }
        public virtual bool CanExecuteUpdateConfirm(object param)
        {
            return IsInUpdateMode;
        }


        // when override this please call the base method!!!
        protected virtual void PopUpMessageClosed(object sender, EventArgs e)
        {
            PopUpMessage.Instance.PopUpMessageClosed -= PopUpMessageClosed;
        }
    }
}
