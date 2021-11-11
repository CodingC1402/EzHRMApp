﻿using DAL.Others;
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

        public bool CheckingIn(DateTime checkInStamp)
        {
            DateTime gioTanLam = checkInStamp.Date;
            TimeSpan time;
            if (gioTanLam.DayOfWeek == DayOfWeek.Saturday)
                time = TimetableRepo.CurrentTimetable.GioTanLamThuBay;
            else if (gioTanLam.DayOfWeek == DayOfWeek.Sunday)
                time = TimetableRepo.CurrentTimetable.GioTanLamChuNhat;
            else
                time = TimetableRepo.CurrentTimetable.GioTanLamCacNgayTrongTuan;

            gioTanLam = gioTanLam.Add(time);
            if (checkInStamp > gioTanLam)
                return false;

            using (var uow = new UnitOfWork())
            {
                var newestCheckIn = CheckInRepo.Instance.FindNewestCheckIn(ID);

                if (!newestCheckIn.ThoiGianTanLam.HasValue)
                    return false;

                CheckInRepo.Instance.Add(new CheckIn { 
                    ThoiGianVaoLam = checkInStamp,
                    IDNhanVien = ID
                }, uow);

                return uow.Complete();
            }
        }

        public bool CheckingOut(DateTime checkOutStamp)
        {
            var newestCheckIn = CheckInRepo.Instance.FindNewestCheckIn(ID);
            if (newestCheckIn.ThoiGianTanLam.HasValue)
                return false;

            DateTime gioVaoLam = newestCheckIn.ThoiGianVaoLam.Date;
            TimeSpan time;
            if (gioVaoLam.DayOfWeek == DayOfWeek.Saturday)
                time = TimetableRepo.CurrentTimetable.GioVaoLamThuBay;
            else if (gioVaoLam.DayOfWeek == DayOfWeek.Sunday)
                time = TimetableRepo.CurrentTimetable.GioVaoLamChuNhat;
            else
                time = TimetableRepo.CurrentTimetable.GioVaoLamCacNgayTrongTuan;

            gioVaoLam = gioVaoLam.Add(time);

            using (var uow = new UnitOfWork())
            {
                if (checkOutStamp < gioVaoLam)
                    CheckInRepo.Instance.Remove(new object[] { newestCheckIn.ThoiGianVaoLam, newestCheckIn.IDNhanVien }, uow);
                else
                {
                    newestCheckIn.ThoiGianTanLam = checkOutStamp;
                    newestCheckIn.Save(uow);

                    DateTime gioTanLam = newestCheckIn.ThoiGianVaoLam.Date;
                    if (gioVaoLam.DayOfWeek == DayOfWeek.Saturday)
                        time = TimetableRepo.CurrentTimetable.GioTanLamThuBay;
                    else if (gioVaoLam.DayOfWeek == DayOfWeek.Sunday)
                        time = TimetableRepo.CurrentTimetable.GioTanLamChuNhat;
                    else
                        time = TimetableRepo.CurrentTimetable.GioTanLamCacNgayTrongTuan;

                    gioTanLam = gioTanLam.Add(time);
                    DateTime batDauLamViec = newestCheckIn.ThoiGianVaoLam > gioVaoLam ? newestCheckIn.ThoiGianVaoLam : gioVaoLam;
                    DateTime ketThucLamViecTrongGio = checkOutStamp < gioTanLam ? checkOutStamp : gioTanLam;
                    WorkhoursInDay soGioLam = new WorkhoursInDay()
                    {
                        Ngay = newestCheckIn.ThoiGianVaoLam.Date,
                        IDNhanVien = newestCheckIn.IDNhanVien,
                        SoGioLamTrongGio = (ketThucLamViecTrongGio - batDauLamViec).Hours,
                        SoGioLamNgoaiGio = (checkOutStamp - ketThucLamViecTrongGio).Hours
                    };
                    soGioLam.Save(uow);
                }
                
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
                    EmployeeRepo.Instance.Update(new object[] { ID }, this, uowNew);
                    return uowNew.Complete();
                }
            }

            return EmployeeRepo.Instance.Update(new object[] { ID }, this, uow);
        }
    }
}
