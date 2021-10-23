using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Rows
{
    public class Department : Row
    {
        public int ID { get; set; }
        public string TenPhong { get; set; }
        public string TruongPhong { get; set; }
    }
}
