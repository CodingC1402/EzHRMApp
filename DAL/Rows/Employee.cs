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
                
                var checkReport = CheckReportRepo.Instance.FindCurrentDateReport();
                var variables = VariablesRepo.Instance.GetLatestVariables();
                DateTime gioVaoLam = checkInStamp.Date;
                if (gioVaoLam.DayOfWeek == DayOfWeek.Saturday)
                    time = TimetableRepo.CurrentTimetable.GioVaoLamThuBay;
                else if (gioVaoLam.DayOfWeek == DayOfWeek.Sunday)
                    time = TimetableRepo.CurrentTimetable.GioVaoLamChuNhat;
                else
                    time = TimetableRepo.CurrentTimetable.GioVaoLamCacNgayTrongTuan;

                gioVaoLam = gioVaoLam.Add(time);
                if (checkInStamp < gioVaoLam)
                    checkReport.SoNVDenSom++;
                else if (checkInStamp < gioVaoLam.Add(variables.ThoiGianChoPhepDiTre))
                    checkReport.SoNVDenDungGio++;
                else
                    checkReport.SoNVDenTre++;

                checkReport.Save(uow);
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
                {
                    CheckInRepo.Instance.Remove(new object[] { newestCheckIn.ThoiGianVaoLam, newestCheckIn.IDNhanVien }, uow);
                    if (newestCheckIn.ThoiGianVaoLam < gioVaoLam)
                    {
                        var checkReport = CheckReportRepo.Instance.FindCurrentDateReport();
                        checkReport.SoNVDenSom--;
                        checkReport.Save(uow);
                    }
                }
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
                    var thoiGianLamTrongGio = ketThucLamViecTrongGio - batDauLamViec;
                    var thoiGianLamNgoaiGio = checkOutStamp - ketThucLamViecTrongGio;
                    WorkhoursInDay soGioLam = new WorkhoursInDay()
                    {
                        Ngay = newestCheckIn.ThoiGianVaoLam.Date,
                        IDNhanVien = newestCheckIn.IDNhanVien,
                        SoGioLamTrongGio = thoiGianLamTrongGio.Hours + thoiGianLamTrongGio.Minutes * 0.01f,
                        SoGioLamNgoaiGio = thoiGianLamNgoaiGio.Hours + thoiGianLamNgoaiGio.Minutes * 0.01f
                    };
                    soGioLam.Save(uow);
                }
                
                return uow.Complete();
            }
        }

        public virtual ProfilePicture GetProfilePicture()
        {
            return ProfilePictureRepo.Instance.FindByID(ID);
        }

        public override string Add(UnitOfWork uow = null)
        {
            if (uow == null)
            {
                using (var uowNew = new UnitOfWork())
                {
                    EmployeeRepo.Instance.Add(this, uowNew);
                    return ExecuteAndReturn(uowNew);
                }
            }

            if (EmployeeRepo.Instance.Add(this, uow))
                return "";
            else
                return "Failed!";
        }

        public override string Save(UnitOfWork uow = null)
        {
            if (uow == null)
            {
                using (var uowNew = new UnitOfWork())
                {
                    EmployeeRepo.Instance.Update(new object[] { ID }, this, uowNew);
                    return ExecuteAndReturn(uowNew);
                }
            }

            if (EmployeeRepo.Instance.Update(new object[] { ID }, this, uow))
                return "";
            else
                return "Failed!";
        }

        public override string CheckForError()
        {
            try
            {
                if (ChucVu == null)
                    return "Position can't be empty!";

                if (NgaySinh > NgayVaoLam)
                {
                    return "Date of birth can't be greater than working date!";
                }

                if (NgayThoiViec.HasValue && NgayThoiViec.Value < NgayVaoLam)
                {
                    return "Resign date can't be before working date!";
                }

                if (Ten == null || Ten == "")
                {
                    return "Name can't be empty!";
                }

                if (CMND == null || CMND == "")
                {
                    return "Citizen ID can't be empty!";
                }

                if (EmailVanPhong == null || EmailVanPhong == "")
                    return "Work email can't be empty!";

                if (SDTVanPhong == null || SDTVanPhong == "")
                    return "Work phone can't be empty!";

                if (!IsValidEmail(EmailVanPhong))
                    return "Work email isn't valid!";

                if (!IsValidEmail(EmailCaNhan))
                    return "Personal email isn't valid!";

                if (!IsValidCitizenID(CMND))
                    return "Citizen ID is not valid!";

                return "";
            }
            catch(Exception e) 
            { 
                return $"Unknow error: {e.Message}"; 
            }
        }

        public bool IsValidEmail(string email)
        {
            if (email == null || email.Trim().EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public bool IsValidCitizenID(string id)
        {
            if (id == null)
                return false;
                
            bool result = id.Length == 9 || id.Length == 12;
            foreach (char c in id)
            {
                if (!char.IsDigit(c))
                {
                    result &= false;
                }
            }
            return result;
        }
    }
}
