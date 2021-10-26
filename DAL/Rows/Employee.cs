using DAL.Others;
using DAL.Repos;
using SqlKata.Execution;
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
        public string PhongBan { get; set; }
        public string ChucVu { get; set; }
        public string TaiKhoan {get; set;}

        public Employee() { }
        public Employee (Employee employee)
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

    public bool ChangeDepartment(Department department)
        {
            this.PhongBan = department.TenPhong;
            using (var uow = new UnitOfWork())
            {
                Save(uow);
                return uow.Complete();
            }
        }

        public bool ChangeRole(Role role)
        {
            this.ChucVu = role.TenChucVu;
            using (var uow = new UnitOfWork())
            {
                Save(uow);
                return uow.Complete();
            }
        }

        public bool Leave(DateTime leaveDate, string reason, int numberOfDays)
        {
            using (var uow = new UnitOfWork())
            {
                uow.Repos.Add(LeaveRepo.Instance.TableName, new List<Row>(LeaveRepo.Instance.FindBy(nameof(Rows.Leave.IDNhanVien), ID)));
                foreach (Leave leave in uow.Repos[LeaveRepo.Instance.TableName])
                {
                    if (leave.NgayBatDauNghi == leaveDate)
                    {
                        return false;
                    }
                }

                LeaveRepo.Instance.Add(new Rows.Leave { 
                    IDNhanVien = ID,
                    LyDoNghi = reason,
                    SoNgayNghi = numberOfDays,
                    NgayBatDauNghi = leaveDate
                }, uow);

                return uow.Complete();
            }
        }

        public bool CheckingIn(DateTime checkIn)
        {
            using (var uow = new UnitOfWork())
            {
                var db = DatabaseConnector.Database;
                var newestCheckIn = CheckInRepo.Instance.FindNewestCheckIn(ID);

                if (!newestCheckIn.ThoiGianTanLam.HasValue)
                    return false;

                CheckInRepo.Instance.Add(new CheckIn { 
                    ThoiGianVaoLam = checkIn,
                    IDNhanVien = ID
                }, uow);

                return uow.Complete();
            }
        }

        public bool CheckingOut(DateTime checkOut)
        {
            using (var uow = new UnitOfWork())
            {
                var db = DatabaseConnector.Database;
                var newestCheckIn = CheckInRepo.Instance.FindNewestCheckIn(ID);

                if (newestCheckIn.ThoiGianTanLam.HasValue)
                    return false;

                newestCheckIn.ThoiGianTanLam = checkOut;
                newestCheckIn.Save(uow);

                return uow.Complete();
            }
        }

        public virtual ProfilePicture GetProfilePicture()
        {
            return ProfilePictureRepo.Instance.FindByID(new object[] { ID });
        }

        public override bool Save(UnitOfWork uow = null)
        {
            if (uow == null)
            {
                using (var uowNew = new UnitOfWork())
                {
                    EmployeeRepo.Instance.Update(new object[] { ID }, this, uow);
                    return uowNew.Complete();
                }
            }

            return EmployeeRepo.Instance.Update(new object[] { ID }, this, uow);
        }
    }
}
