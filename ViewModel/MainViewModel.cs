using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Navigation;

namespace ViewModel
{
    public class MainViewModel : Navigation.ViewModelBase
    {
        public override string ViewName => "Main";

        private List<ViewModelBase> _viewModels = new List<ViewModelBase>();
        public List<ViewModelBase> ViewModels { get => _viewModels; }

        public ViewModelBase CurrentViewModel { get; set; }

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

            ToLogin = new NavigationCommand<LoginViewModel>(new LoginViewModel());
            _viewModels.Add(ToLogin.ViewModel);

            ToLoggedIn = new NavigationCommand<LoggedInViewModel>(new LoggedInViewModel());
            _viewModels.Add(ToLoggedIn.ViewModel);

            CurrentViewModel = ToLogin.ViewModel;
        }

        public NavigationCommand<LoginViewModel> ToLogin { get; set; }
        public NavigationCommand<LoggedInViewModel> ToLoggedIn { get; set; }
    }
}
