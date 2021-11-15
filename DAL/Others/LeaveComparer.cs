using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Others
{
    // Slow but surely remove the annoying Time
    public class LeaveComparer : IComparer<Leave>
    {
        public int Compare(Leave leave1, Leave leave2)
        {
            return leave1.NgayBatDauNghi.Date.CompareTo(leave2.NgayBatDauNghi.Date);
        }
    }
}
