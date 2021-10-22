using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Helper;
using ViewModel.Navigation;

namespace ViewModel
{
    public class StaffsManagementViewModel : Navigation.NavigationViewModel
    {
        public override string ViewName => "Staffs Management";

        public StaffsManagementViewModel()
        {
            ToStaffsView = new NavigationCommand<StaffsViewModel>(new StaffsViewModel(), this, 0);
            ViewModels.Add(ToStaffsView.ViewModel);

            CurrentViewModel = ToStaffsView.ViewModel;
        }

        public NavigationCommand<StaffsViewModel> ToStaffsView { get; set; }
    }
}
