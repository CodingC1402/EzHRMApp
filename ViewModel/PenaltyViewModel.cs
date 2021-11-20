using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel
{
    public class PenaltyViewModel : Navigation.CRUDViewModelBase
    {
        public override string ViewName => "Penalty";

        public static PenaltyViewModel Instance { get; private set; }

        public PenaltyViewModel()
        {
            Instance = this;
        }
    }
}
