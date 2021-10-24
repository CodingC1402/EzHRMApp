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
            public string ID { get; set; }
            public string Name { get; set; }
            public string CMND { get; set; }
            public string BirthDay { get; set; }
            public string PrivateEmail { get; set; }
            public string OfficeEmail { get; set; }
            public string PrivatePhoneNumber { get; set; }
            public string OfficePhoneNumber{ get; set; }
            public string InDate { get; set; }
            public string OutDate { get; set; }
            public string Department { get; set; }
            public string Role { get; set; }

            public int DepartmentID { get; set; }
            public int RoleID { get; set; }

            public Employee() { }
            
            public Employee(Employee input)
            {
                this.ID = input.ID;
                this.Name = input.Name;
                this.CMND = input.CMND;
                this.BirthDay = input.BirthDay;
                this.PrivateEmail = input.PrivateEmail;
                this.OfficeEmail = input.OfficeEmail;
                this.PrivatePhoneNumber = input.PrivatePhoneNumber;
                this.OfficePhoneNumber = input.OfficePhoneNumber;
                this.InDate = input.InDate;
                this.OutDate = input.OutDate;
                this.Department = input.Department;
                this.Role = input.Role;

                this.DepartmentID = input.DepartmentID;
                this.RoleID = input.RoleID;
            }
        }

        public static ObservableCollection<Employee> GetList()
        {
            ObservableCollection<Employee> result = new ObservableCollection<Employee>();
            List<DAL.Rows.Employee> datas = new List<DAL.Rows.Employee>(EmployeeRepo.Instance.GetAll());
            Employee employee;

            foreach (DAL.Rows.Employee data in datas)
            {
                employee = new Employee();
                employee.ID = data.ID;
                employee.Name = data.Ho + " " + data.Ten;
                employee.CMND = data.CMND;
                employee.BirthDay = data.NgaySinh.ToShortDateString();
                employee.PrivateEmail = data.EmailCaNhan;
                employee.OfficeEmail = data.EmailVanPhong;
                employee.PrivatePhoneNumber = data.SDTCaNhan;
                employee.OfficePhoneNumber = data.SDTVanPhong;
                employee.InDate = data.NgayVaoLam.ToShortDateString();

                if (data.NgayThoiViec.HasValue)
                    employee.OutDate = data.NgayThoiViec.Value.ToShortDateString();

                employee.Department = DepartmentRepo.Instance.FindByID(data.PhongBan).TenPhong;
                employee.Role = RoleRepo.Instance.FindByID(data.ChucVu).TenChucVu;


                employee.DepartmentID = data.PhongBan;
                employee.RoleID = data.ChucVu;
                result.Add(employee);
            }

            return result;
        }

        public static void AddStaff(Employee input)
        {
            DAL.Rows.Employee employee = new DAL.Rows.Employee();

            employee.ID = input.ID;
            string[] temp = input.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            employee.Ho = temp[0];
            employee.Ten = temp.Length == 1 ? temp[0] : temp[1];
            employee.CMND = input.CMND;
            employee.NgaySinh = DateTime.Parse(input.BirthDay);
            employee.EmailCaNhan = input.PrivateEmail;
            employee.EmailVanPhong = input.OfficeEmail;
            employee.SDTCaNhan = input.PrivatePhoneNumber;
            employee.SDTVanPhong = input.OfficePhoneNumber;
            employee.NgayVaoLam = DateTime.Parse(input.InDate);
            employee.PhongBan = input.DepartmentID;
            employee.ChucVu = input.RoleID;

            EmployeeRepo.Instance.Add(employee);
        }

        public static void UpdateStaff(Employee input)
        {
            DAL.Rows.Employee employee = new DAL.Rows.Employee();

            employee.ID = input.ID;
            string[] temp = input.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            employee.Ho = temp[0];
            employee.Ten = temp.Length == 1 ? temp[0] : temp[1];
            employee.CMND = input.CMND;
            employee.NgaySinh = DateTime.Parse(input.BirthDay);
            employee.EmailCaNhan = input.PrivateEmail;
            employee.EmailVanPhong = input.OfficeEmail;
            employee.SDTCaNhan = input.PrivatePhoneNumber;
            employee.SDTVanPhong = input.OfficePhoneNumber;
            employee.NgayVaoLam = DateTime.Parse(input.InDate);
            employee.PhongBan = input.DepartmentID;
            employee.ChucVu = input.RoleID;

            EmployeeRepo.Instance.Update(employee.ID, employee);
        }

        public static bool CheckStaffID(Employee input)
        {
            if (EmployeeRepo.Instance.FindByID(input.ID) != null)
                return true;

            return false;
        }

        public static bool CheckStaffInfo(Employee input)
        {
            if (DepartmentRepo.Instance.FindByID(input.DepartmentID) == null)
                return true;

            if (RoleRepo.Instance.FindByID(input.RoleID) == null)
                return true;

            return false;
        }
    }
}
