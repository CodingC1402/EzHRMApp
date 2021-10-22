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
        public int PhongBan { get; set; }
        public int ChucVu { get; set; }
        public string TaiKhoan {get; set;}

        public bool ChangeDepartment(Department department)
        {
            this.PhongBan = department.ID;
            using (var uow = new UnitOfWork())
            {
                Save(uow);
                return uow.Complete();
            }
        }

        public bool ChangeRole(Role role)
        {
            this.ChucVu = role.ID;
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

        public override bool Save(UnitOfWork uow)
        {
            return EmployeeRepo.Instance.Update(new object[] { ID }, this, uow);
        }
    }
}
