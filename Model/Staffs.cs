using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Data;
using DAL.Repos;
using DAL.Rows;

namespace Model
{
    public class Staffs
    {
        public class Employee : BaseModel
        {
            public string Name { get; set; }
            public string EmailCaNhan { get; set; }
            public string EmailVanPhong { get; set; }
            public string CMND { get; set; }
            public string Department { get; set; }
            public string Role { get; set; }
        }

        public static ObservableCollection<Employee> GetList()
        {
            ObservableCollection<Employee> result = new ObservableCollection<Employee>();
            List<DAL.Rows.Employee> datas = new List<DAL.Rows.Employee>(EmployeeRepo.Instance.GetAll());
            Employee employee;

            foreach (DAL.Rows.Employee data in datas)
            {
                employee = new Employee();
                employee.Name = data.Ho + data.Ten;
                employee.EmailCaNhan = data.EmailCaNhan;
                employee.EmailVanPhong = data.EmailVanPhong;
                employee.CMND = data.CMND;

                employee.Department = DepartmentRepo.Instance.FindByID(data.PhongBan).TenPhong;
                employee.Role = RoleRepo.Instance.FindByID(data.ChucVu).TenChucVu;

                result.Add(employee);
            }

            return result;
        }
    }
}
