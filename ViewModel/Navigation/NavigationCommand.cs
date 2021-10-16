using System;
using System.Windows.Input;
using ViewModel;

namespace ViewModel.Navigation
{
    public class NavigationCommand<T> : ICommand where T : ViewModelBase
    {
        public event EventHandler CanExecuteChanged;

        private NavigationViewModel _owner = null;
        private T _viewModel = null;
        public T ViewModel { get => _viewModel; }

        public NavigationCommand (T viewModel, NavigationViewModel owner) 
        {
            _viewModel = viewModel;
            _owner = owner;
        }

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public virtual void Execute(object parameter)
        {
           _owner.CurrentViewModel = _viewModel;
        }
    }
}
