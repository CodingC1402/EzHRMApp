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
            var rows = EmployeeRepo.Instance.GetAll();
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

        public static EmployeeModel GetEmployeeByID(string id)
        {
            var employee = EmployeeRepo.Instance.FindByID(new object[] { id });

            if (employee == null)
                return null;

            return new EmployeeModel(employee);
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
            string result = "";

            if (addNew)
            {
                UnitOfWork uow = new UnitOfWork();
                Employee employee = new Employee(this);
                employee.TaiKhoan = employee.ID;

                result = employee.CheckForError();
                if (result != "")
                    return result;

                RoleModel role = RoleModel.GetRole(employee.ChucVu);
                Account account = Account.CreateAccount(employee.ID, role.NhomTaiKhoan);
                if (account != null)
                {
                    AccountRepo.Instance.Add(account, uow);
                }
                EmployeeRepo.Instance.Add(employee, uow);

                result = uow.Complete() ? "" : "Transaction failed! Unknow reason";
            }
            else
                result = Save();

            if (result == "" && ProfilePicture != null)
            {
                result = ProfilePicture.Save();
            }

            return result;
        }

        protected new string Save(UnitOfWork unitOfWork = null)
        {
            Employee row = new Employee(this);
            return row.Save();
        }
    }
}
