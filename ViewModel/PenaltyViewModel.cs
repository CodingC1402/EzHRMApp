using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ViewModel
{
    public class PenaltyViewModel : Navigation.ViewModelBase
    {
        public override string ViewName => "Penalty";

        public static PenaltyViewModel Instance { get; private set; }
        public ObservableCollection<PenaltyModel> Penalties { get; set; }
        public PenaltyModel SelectedPenalty { get; set; }


        public PenaltyViewModel()
        {
            Instance = this;
            Penalties = PenaltyModel.LoadAll();
        }
    }
}
