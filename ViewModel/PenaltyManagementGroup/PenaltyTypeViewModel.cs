using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ViewModel
{
    public class PenaltyTypeViewModel : Navigation.CRUDViewModelBase
    {
        public override string ViewName => "Penalty Type";

        public static PenaltyTypeViewModel Instance { get; private set; }
        public ObservableCollection<PenaltyTypeModel> PenaltyTypes { get; set; }

        public PenaltyTypeViewModel()
        {
            Instance = this;
            PenaltyTypes = PenaltyTypeModel.LoadAll();
        }
    }
}
