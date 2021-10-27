using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Data;
using DAL.Repos;
using DAL.Rows;

namespace Model
{
    public class EmployeeModel : Employee
    {
        public EmployeeModel() { }
        public EmployeeModel(Employee employee) : base(employee) { }
        public EmployeeModel(EmployeeModel employee) : base(employee)
        {

        }

        public static ObservableCollection<Employee> GetList()
        {
            ObservableCollection<Employee> result = new ObservableCollection<Employee>();
            List<Employee> datas = new List<Employee>(EmployeeRepo.Instance.GetAll());

            foreach (Employee data in datas)
            {
                result.Add(data);
            }

            return result;
        }

        public static void AddStaff(Employee input, string accessToken)
        {
            int bitmask = AccessTokenRepo.Instance.FindByID(accessToken).Bitmask;

            if (((AccountRepo.Privileges)bitmask & AccountRepo.Privileges.SearchAndEditEmployee) != AccountRepo.Privileges.SearchAndEditEmployee)
            {
                return;
            }

            EmployeeRepo.Instance.Add(input);
        }

        public static void UpdateStaff(Employee input, string accessToken)
        {
            int bitmask = AccessTokenRepo.Instance.FindByID(accessToken).Bitmask;
            
            if (((AccountRepo.Privileges)bitmask & AccountRepo.Privileges.SearchAndEditEmployee) != AccountRepo.Privileges.SearchAndEditEmployee)
            {
                return;
            }

            EmployeeRepo.Instance.Update(input.ID, input);
        }

        public static bool CheckStaffID(Employee input)
        {
            if (EmployeeRepo.Instance.FindByID(input.ID) != null)
                return true;

            return false;
        }

        public static bool CheckStaffInfo(Employee input)
        {
            if (DepartmentRepo.Instance.FindByID(input.PhongBan) == null)
                return true;

            if (RoleRepo.Instance.FindByID(input.ChucVu) == null)
                return true;

            return false;
        }
    }
}
