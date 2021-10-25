using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using PropertyChanged;
using DAL.Rows;
using System.Collections.ObjectModel;
using DAL.Repos;
using DAL.Others;

namespace Model
{
    public class EmployeeModel : Employee
    {
        public enum SaveResult
        {
            Ok = 0,
            FailProfile = 1,
            FailData = 2,
            FailAll = 3,
        }

        public static ObservableCollection<EmployeeModel> LoadAll()
        {
            var rows = DAL.Repos.EmployeeRepo.Instance.GetAll();
            var result = new ObservableCollection<EmployeeModel>();

            foreach (var row in rows)
            {
                result.Add(new EmployeeModel(row));
            }
            return result;
        }

        public ProfilePicture ProfilePicture { get; set; }

        public EmployeeModel() { }
        public EmployeeModel(Employee employee) : base(employee) { }
        public EmployeeModel(EmployeeModel employee) : base(employee)
        {
            var profile = employee.ProfilePicture;
            if (profile != null)
            {
                ProfilePicture = new ProfilePicture()
                {
                    ID = profile.ID,
                    Image = (byte[])profile.Image.Clone(),
                    Type = profile.Type,
                    Width = profile.Width
                };
            }
        }

        public static string GetNextEmployeeID()
        {
            return EmployeeRepo.Instance.GetNextID();
        }

        public override ProfilePicture GetProfilePicture()
        {
            if (ProfilePicture != null)
                return ProfilePicture;

            var profile = base.GetProfilePicture();
            ProfilePicture = profile ?? (new ProfilePicture { ID = ID });
            return ProfilePicture;
        }

        public SaveResult Save()
        {
            var result = SaveResult.Ok;
            if (ProfilePicture != null && !ProfilePicture.Save())
                result |= SaveResult.FailProfile;
            if (!Save(null))
                result |= SaveResult.FailData;

            return result;
        }

        protected new bool Save(UnitOfWork unitOfWork = null)
        {
            return base.Save();
        }
    }
}
