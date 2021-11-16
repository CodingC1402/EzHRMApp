using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Rows
{
    public class DailyReport : Row
    {
        public DateTime NgayBaoCao { get; set; }
        public int SoNVDenSom { get; set; }
        public int SoNVDenDungGio { get; set; }
        public int SoNVDenTre { get; set; }
        public int SoNVTanLamSom { get; set; }
        public int SoNVTanLamDungGio { get; set; }
        public int SoNLamThemGio { get; set; }

        public DailyReport() { }
        public DailyReport(DailyReport report)
        {
            NgayBaoCao = report.NgayBaoCao;
            SoNVDenSom = report.SoNVDenSom;
            SoNVDenDungGio = report.SoNVDenDungGio;
            SoNVDenTre = report.SoNVDenTre;
            SoNVTanLamSom = report.SoNVTanLamSom;
            SoNVTanLamDungGio = report.SoNVTanLamDungGio;
            SoNLamThemGio = report.SoNLamThemGio;
        }
    }
}
