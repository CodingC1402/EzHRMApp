using ViewModel.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Security;
using DAL;
using System.Windows;

namespace ViewModel
{
    public class LoginViewModel : Navigation.ViewModelBase
    {
        public override string ViewName => "Login";

        public string UserName { get; set; }

        protected RelayCommand<object> _loginRelayCommand;
        public ICommand LoginCommand => _loginRelayCommand ?? (_loginRelayCommand = new RelayCommand<object>(ExecuteLogin, CanExecuteLogin)) ;

        // Send in password
        protected void ExecuteLogin(object param)
        {
            LoginInfo.Login(UserName, param as SecureString);
        }
        protected bool CanExecuteLogin(object param)
        {
            return true;
        }

        public static void LogOut()
        {
            LoginInfo.Logout();
        }
    }
}
