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

            ToHomeView = new NavigationCommand<HomeViewModel>(new HomeViewModel());
            _viewModels.Add(ToHomeView.ViewModel);

            ToDashboard = new NavigationCommand<DashboardViewModel>(new DashboardViewModel());
            _viewModels.Add(ToHomeView.ViewModel);

            ToStaffsManagementView = new NavigationCommand<StaffsManagementViewModel>(new StaffsManagementViewModel());
            _viewModels.Add(ToHomeView.ViewModel);

            CurrentViewModel = ToHomeView.ViewModel;
        }

        public NavigationCommand<HomeViewModel> ToHomeView { get; set; }
        public NavigationCommand<DashboardViewModel> ToDashboard { get; set; }
        public NavigationCommand<StaffsManagementViewModel> ToStaffsManagementView { get; set; }
    }
}
