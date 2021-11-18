using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel
{
    public class PersonalViewModel : Navigation.ViewModelBase
    {
        public override string ViewName => "Personal";

        public static PersonalViewModel Instance { get; private set; }

        public PersonalViewModel()
        {
            Instance = this;
        }
    }
}
