using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Rows
{
    public class MonthlyReport : Row
    {
        public int Thang { get; set; }
        public int Nam { get; set; }
        public int SoNhanVienMoi { get; set; }
        public int SoNhanVienThoiViec { get; set; }

        public MonthlyReport() { }
        
        public MonthlyReport(MonthlyReport report)
        {
            Thang = report.Thang;
            Nam = report.Nam;
            SoNhanVienMoi = report.SoNhanVienMoi;
            SoNhanVienThoiViec = report.SoNhanVienThoiViec;
        }
    }
}
