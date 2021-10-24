using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Rows
{
    public class Leave : Row
    {
        public string IDNhanVien { get; set; }
        public DateTime NgayBatDauNghi { get; set; }
        public int SoNgayNghi { get; set;  }
        public string LyDoNghi { get; set; }
    }
}
