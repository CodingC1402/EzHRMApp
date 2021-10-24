using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using PropertyChanged;
using DAL.Rows;
using System.Collections.ObjectModel;

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

        public EmployeeModel(Employee employee)
        {
            ID = employee.ID;
            Ho = employee.Ho;
            Ten = employee.Ten;
            CMND = employee.CMND;
            NgaySinh = employee.NgaySinh;
            EmailVanPhong = employee.EmailVanPhong;
            EmailCaNhan = employee.EmailCaNhan;
            SDTVanPhong = employee.SDTVanPhong;
            SDTCaNhan = employee.SDTCaNhan;
            NgayVaoLam = employee.NgayVaoLam;
            NgayThoiViec = employee.NgayThoiViec;
            PhongBan = employee.PhongBan;
            ChucVu = employee.ChucVu;
            TaiKhoan = employee.TaiKhoan;
        }
    }
}
