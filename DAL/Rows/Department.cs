using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Rows
{
    public class Department : Row
    {
        public string TenPhong { get; set; }
        public DateTime NgayThanhLap { get; set; }
        public DateTime NgayNgungHoatDong { get; set; }
        public string TruongPhong { get; set; }
    }
}
