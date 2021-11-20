using DAL.Others;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Rows
{
    public class CompanySchedule : Row
    {
        public DateTime ThoiDiemTao { get; set; }

        public TimeSpan GioVaoLamCacNgayTrongTuan { get; set; }
        public TimeSpan GioVaoLamThuBay { get; set; }
        public TimeSpan GioVaoLamChuNhat { get; set; }

        public TimeSpan GioTanLamCacNgayTrongTuan { get; set; }
        public TimeSpan GioTanLamThuBay { get; set; }
        public TimeSpan GioTanLamChuNhat { get; set; }

        public CompanySchedule() { }
        public CompanySchedule(CompanySchedule companySchedule)
        {
            ThoiDiemTao = companySchedule.ThoiDiemTao;
            GioVaoLamCacNgayTrongTuan = companySchedule.GioVaoLamCacNgayTrongTuan;
            GioVaoLamChuNhat = companySchedule.GioVaoLamChuNhat;
            GioVaoLamThuBay = companySchedule.GioVaoLamThuBay;

            GioTanLamCacNgayTrongTuan = companySchedule.GioTanLamCacNgayTrongTuan;
            GioTanLamThuBay = companySchedule.GioTanLamThuBay;
            GioTanLamChuNhat = companySchedule.GioTanLamChuNhat;
        }

        public override string Save(UnitOfWork uow)
        {
            var result = this.CheckForError();

            if (result != "")
                return result;

            if (uow == null)
            {
                using (var uowNew = new UnitOfWork())
                {
                    CompanyScheduleRepo.Instance.Add(this, uowNew);
                    return ExecuteAndReturn(uowNew);
                }
            }

            if (CompanyScheduleRepo.Instance.Add(this, uow))
                return "";
            else
                return "Failed!";
        }

        public override string Add(UnitOfWork uow = null)
        {
            return Save(uow);
        }
    }
}
