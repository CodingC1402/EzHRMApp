using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel
{
    public class PersonalViewModel : Navigation.ViewModelBase
    {
        public override string ViewName => "Personal";

        public static PersonalViewModel Instance { get; private set; }

        public EmployeeModel UserInfo { get; set; }

        public PersonalViewModel()
        {
            Instance = this;
            UserInfo = EmployeeModel.GetEmployeeByID(LoginInfo.EmployeeID);
        }
    }
}
