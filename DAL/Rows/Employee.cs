using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Rows
{
    public class Employee : Row
    {
        public string ID { get; set; }
        public string Ho { get; set; }
        public string Ten { get; set; }
        public string CMND { get; set; }
        public DateTime NgaySinh { get; set; }
        public string EmailVanPhong { get; set; }
        public string EmailCaNhan { get; set; }
        public string SDTVanPhong { get; set; }
        public string SDTCaNhan { get; set; }
        public DateTime NgayVaoLam { get; set; }
        public DateTime? NgayThoiViec { get; set; }
        public int PhongBan { get; set; }
        public int ChucVu { get; set; }
        public string TaiKhoan { get; set; }

        public Employee() { }
        public Employee(Employee employee)
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
