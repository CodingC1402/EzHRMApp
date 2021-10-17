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
        public string Result { get; set; } = "";

        protected RelayCommand<object> _loginRelayCommand;
        public ICommand LoginCommand => _loginRelayCommand ?? (_loginRelayCommand = new RelayCommand<object>(ExecuteLogin, CanExecuteLogin)) ;

        // Send in password
        protected void ExecuteLogin(object param)
        {
            Result = "";
            if (UserName == null || param is null || UserName == "")
            {
                Result = "Username and password can't not be empty!";
                return;
            }

            if ((param as SecureString).Length < LoginInfo.MinPasswordLength)
            {
                Result = $"Password can't not be short than {LoginInfo.MinPasswordLength} characters!";
                return;
            }

            if (LoginInfo.Login(UserName, param as SecureString))
            {
                var mainVM = MainViewModel.GetInstance();
                mainVM.CurrentViewModel = mainVM.ToLoggedIn.ViewModel;
            }
            else
            {
                Result = "Username or Password is wrong!";
            }
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
