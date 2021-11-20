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

        public override void ExecuteUpdate(object param)
        {
            base.ExecuteUpdate(param);
        }

        public override void ExecuteConfirmUpdate(object param)
        {
            var result = UserInfo.Save();

            if (result == "")
            {
                base.ExecuteConfirmUpdate(param);
                StartUpdateCommand.RaiseCanExecuteChangeEvent();
            }
            else
            {
                ErrorString = result;
                HaveError = true;
            }
        }

        public override void ExecuteCancleUpdate(object param)
        {
            base.ExecuteCancleUpdate(param);
            StartUpdateCommand.RaiseCanExecuteChangeEvent();
        }

        public override bool CanExecuteUpdateStart(object param)
        {
            return base.CanExecuteUpdateStart(param) && UserInfo != null;
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
