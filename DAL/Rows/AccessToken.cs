using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Rows
{
    public class AccessToken : Row
    {
        public string Token { get; set; }
        public int Bitmask { get; set; }
        public string NhanVienID { get; set; }
        public string Account { get; set; }
    }
}
