using System;
using System.Collections.Generic;
using System.Text;
using ViewModel.Navigation;

namespace ViewModel
{
    public class PenaltyManagementViewModel : Navigation.NavigationViewModel
    {
        public override string ViewName => "Penalty Management";

        private static PenaltyManagementViewModel __instance = null;
        public static PenaltyManagementViewModel Instance { get => __instance; }
        public static PenaltyManagementViewModel GetInstance()
        {
            return __instance;
        }

        public PenaltyManagementViewModel()
        {
            __instance = this;

            ToPenaltyView = new NavigationCommand<PenaltyViewModel>(new PenaltyViewModel(), this, 0);
            ViewModels.Add(ToPenaltyView.ViewModel);

            ToPenaltyTypeView = new NavigationCommand<PenaltyTypeViewModel>(new PenaltyTypeViewModel(), this, 0);
            ViewModels.Add(ToPenaltyTypeView.ViewModel);

            ToPenaltyView.Execute(null);
        }

        public NavigationCommand<PenaltyViewModel> ToPenaltyView { get; set; }
        public NavigationCommand<PenaltyTypeViewModel> ToPenaltyTypeView { get; set; }

    }
}
