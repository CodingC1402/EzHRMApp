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
        public static MainViewModel GetInstance()
        {
            return __instance;
        }

        public MainViewModel() {
            if (__instance != null)
            {
                throw new Exception("There is another MainViewModel");
            }
            __instance = this;

            ToLogin = new NavigationCommand<LoginViewModel>(new LoginViewModel(), this);
            ViewModels.Add(ToLogin.ViewModel);

            ToLoggedIn = new NavigationCommand<LoggedInViewModel>(new LoggedInViewModel(), this);
            ViewModels.Add(ToLoggedIn.ViewModel);

            CurrentViewModel = ToLogin.ViewModel;
        }

        public NavigationCommand<LoginViewModel> ToLogin { get; set; }
        public NavigationCommand<LoggedInViewModel> ToLoggedIn { get; set; }
    }
}
