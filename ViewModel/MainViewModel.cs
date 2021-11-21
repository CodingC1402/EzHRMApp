using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Navigation;

namespace ViewModel
{
    public class MainViewModel : Navigation.NavigationViewModel
    {
        public override string ViewName => "Main";

        private static MainViewModel __instance = null;
        public static MainViewModel Instance { get => __instance ?? (__instance = new MainViewModel()); }
        public static MainViewModel GetInstance()
        {
            return Instance;
        }

        public MainViewModel() {
            ToLogin = new NavigationCommand<LoginViewModel>(new LoginViewModel(), this, 0);
            ViewModels.Add(ToLogin.ViewModel);

            ToLoggedIn = new NavigationCommand<LoggedInViewModel>(new LoggedInViewModel(), this, 0);
            ViewModels.Add(ToLoggedIn.ViewModel);

            ToForgotPassword = new NavigationCommand<ForgotPasswordViewModel>(new ForgotPasswordViewModel(), this, 0);
            ViewModels.Add(ToForgotPassword.ViewModel);

            CurrentViewModel = ToLogin.ViewModel;
        }

        public NavigationCommand<LoginViewModel> ToLogin { get; set; }
        public NavigationCommand<LoggedInViewModel> ToLoggedIn { get; set; }
        public NavigationCommand<ForgotPasswordViewModel> ToForgotPassword { get; set; }
    }
}
