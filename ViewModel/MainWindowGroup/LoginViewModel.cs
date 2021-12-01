using ViewModel.Helper;
using System.Windows.Input;
using System.Security;
using Model;

namespace ViewModel
{
    public class LoginViewModel : Navigation.ViewModelBase
    {
        private static LoginViewModel _instance;
        public static LoginViewModel Instance {get => _instance;}
        public override string ViewName => "Login";

        public string UserName { get; set; }
        public string Result { get; set; } = "";

        protected RelayCommand<object> _loginRelayCommand;
        public ICommand LoginCommand => _loginRelayCommand ?? (_loginRelayCommand = new RelayCommand<object>(ExecuteLogin, CanExecuteLogin)) ;

        protected RelayCommand<object> _forgotPasswordCommand;
        public ICommand ForgotPasswordCommand => _forgotPasswordCommand = new RelayCommand<object>(param => {
            if (AccountModel.IsAccountExist(UserName))
            {
                if (EmployeeModel.GetEmployeeFromAccount(UserName) == null)
                {
                    Result = "This is an admin account,\nPlease contact the tech team!";
                    return;
                }

                MainViewModel.Instance.ToForgotPassword.Execute(param);
            }
            else
            {
                Result = "Account doesn't exists";
            }
        });

        public LoginViewModel()
        {
            _instance = this;
        }

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
                mainVM.ToLoggedIn.Execute(param);
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
