using DAL.Others;
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
        public static int GetLeavesDays(Employee employee, DateTime start, DateTime end)
        {
            List<Leave> leaves = new List<Leave>(Instance.GetAll());
            List<Leave> employeesLeaves = new List<Leave>();

            foreach(Leave leave in leaves)
            {
                if (leave.IDNhanVien == employee.ID)
                {
                    var startDate = leave.NgayBatDauNghi;
                    var endDate = startDate.AddDays(leave.SoNgayNghi - 1);

                    if (endDate >= start && endDate < end)
                    {
                        employeesLeaves.Add(leave);
                    }
                }
            }
            employeesLeaves.Sort(new LeaveComparer());

            // Calculating the amount the days in total // Not recounting overlapped
            int total = 0;
            foreach (Leave leave in employeesLeaves)
            {
                var endLeaveDate = leave.NgayBatDauNghi.AddDays(leave.SoNgayNghi - 1);
                endLeaveDate = endLeaveDate > end ? end : endLeaveDate;
                start = leave.NgayBatDauNghi < start ? start : leave.NgayBatDauNghi;

                var numberOfDay = (endLeaveDate - start).Days + 1;

                if (numberOfDay >= 0)
                {
                    total += numberOfDay;
                    start = endLeaveDate.AddDays(1);
                }
            }

            return total;
        }
    }
}
