using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Rows
{
    public class Role : Row
    {
        public int ID { get; set; }
        public string TenChucVu { get; set; }
        public string CachTinhLuong { get; set; }
        public float MucLuongNgoaiGio { get; set; }
    }
}
