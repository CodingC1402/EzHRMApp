using DAL.Others;
using System;
using System.Collections.Generic;
using System.Text;
using DAL.Repos;

namespace DAL.Rows
{
    public class CheckReport : Row
    {
        public DateTime NgayBaoCao { get; set; }
        public int SoNVDenSom { get; set; }
        public int SoNVDenDungGio { get; set; }
        public int SoNVDenTre { get; set; }
        public int SoNVTanLamSom { get; set; }
        public int SoNVTanLamDungGio { get; set; }
        public int SoNVLamThemGio { get; set; }

        public override string Save(UnitOfWork uow)
        {
            return BoolToString(CheckReportRepo.Instance.Update(new object[] { NgayBaoCao }, this, uow));
        }
    }
}
