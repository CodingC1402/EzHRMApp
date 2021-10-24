using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos
{
    public class LeaveRepo : Repo<Leave>
    {
        public override string TableName => "NGHIPHEP";
        LeaveRepo()
        {
            TableName = "NGHIPHEP";
            PKColsName = new string[] { 
                "IDNhanVien",
                "NgayBatDauNghi"
            };
        }

        public static LeaveRepo Instance { get; private set; } = new LeaveRepo();
    }
}
