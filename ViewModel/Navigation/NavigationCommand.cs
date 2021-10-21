using System;
using System.Windows.Input;
using ViewModel;
using Model;

namespace ViewModel.Navigation
{
    public class NavigationCommand<T> : ICommand where T : ViewModelBase
    {
        public event EventHandler CanExecuteChanged;

        private uint _bitMask = 0;
        private NavigationViewModel _owner = null;
        private T _viewModel = null;
        public T ViewModel { get => _viewModel; }


        public NavigationCommand (T viewModel, NavigationViewModel owner, uint bitMask) 
        {
            _viewModel = viewModel;
            _owner = owner;
            _bitMask = bitMask;
        }

        public virtual bool CanExecute(object parameter)
        {
            return (LoginInfo.PrivilegeMask & _bitMask) == _bitMask;
        }

        public virtual void Execute(object parameter)
        {
           _owner.CurrentViewModel = _viewModel;
        }
    }
}
