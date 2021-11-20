using Model;
using System;
using System.Collections.Generic;
using System.Text;
using ViewModel.Helper;
using ViewModel.Structs;

namespace ViewModel
{
    public class UserInfoViewModel : Navigation.CRUDViewModelBase
    {
        public override string ViewName => "User Info";

        public static UserInfoViewModel Instance { get; private set; }

        public EmployeeModel UserInfo { get; set; }

        public Image ProfilePicture { get; set; }

        public bool IsAvailable { get; set; }

        private RelayCommand<object> _selectProfileCommand;
        public RelayCommand<object> SelectProfileCommand => _selectProfileCommand ?? (_selectProfileCommand = new RelayCommand<object>(ExecuteSelectProfile, CanExecuteSelectProfile));

        public bool CanExecuteSelectProfile(object param)
        {
            return true;
        }

        public void ExecuteSelectProfile(object param)
        {
            if (param == null)
                return;

            ProfilePicture = new Image(param as Image);
            var employeeProfile = UserInfo.GetProfilePicture();

            employeeProfile.Image = ProfilePicture.ImageBytes;
            employeeProfile.Width = ProfilePicture.Width;
            employeeProfile.Type = ProfilePicture.FileType;
        }

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
