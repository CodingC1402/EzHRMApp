using Model;
using System;
using System.Collections.Generic;
using System.Text;
using ViewModel.Structs;

namespace ViewModel
{
    public class UserInfoViewModel : Navigation.ViewModelBase
    {
        public override string ViewName => "User Info";

        public static UserInfoViewModel Instance { get; private set; }

        public EmployeeModel UserInfo { get; set; }

        public Image ProfilePicture { get; set; }

        public UserInfoViewModel()
        {
            Instance = this;
            UserInfo = EmployeeModel.GetEmployeeByID(LoginInfo.EmployeeID);
            var profile = UserInfo != null ? UserInfo.GetProfilePicture() : null;
            ProfilePicture = profile != null ? new Image(profile) : null;
        }
    }
}
