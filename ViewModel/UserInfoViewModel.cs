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

        public bool IsAvailable { get; set; }

        public UserInfoViewModel()
        {
            IsAvailable = true;
            Instance = this;
            UserInfo = EmployeeModel.GetEmployeeByID(LoginInfo.EmployeeID);
            
            if (UserInfo == null)
            {
                IsAvailable = false;
            }
            else
            {
                var profile = UserInfo.GetProfilePicture();
                ProfilePicture = profile != null ? new Image(profile) : null;
            }
        }
    }
}
