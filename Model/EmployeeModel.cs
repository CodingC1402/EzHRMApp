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
                    Image = (byte[])profile.Image?.Clone(),
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

        public string Save(bool addNew)
        {
            if (ProfilePicture != null)
            {
                var result = ProfilePicture.Save();
                if (result != "")
                    return result;
            }

            if (addNew)
            {
                Employee row = new Employee(this);
                return row.Add();
            }
            else
                return Save();
        }

        protected new string Save(UnitOfWork unitOfWork = null)
        {
            Employee row = new Employee(this);
            return row.Save();
        }
    }
}
