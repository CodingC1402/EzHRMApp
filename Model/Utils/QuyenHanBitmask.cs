using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Utils
{
    public static class QuyenHanBitmask
    {
        // hex digit 0 (BOSS)
        public static uint Roles = 1;
        public static uint Departments = 2;
        public static uint PenaltyTypes = 4;
        public static uint PayrollTypes = 8;
        public static uint AccountGroups = 16;  // digit 1
        public static uint ScheduleManagement = 32;
        public static uint Holiday = 64;

        // hex digit 2 (HRM)
        public static uint Dashboard = 256;
        public static uint CheckInManagement = 512;
        public static uint Leaves = 1024;
        public static uint Staff = 2048;
        public static uint Penalty = 4096;      // digit 3
        public static uint EmployeePayroll = 8192;
        public static uint Reports = 16384;

        // hex digit 4 (EMPLOYEE)
        public static uint WeeklySchedule = 65536;

        // hex digit 5
        public static uint UserInfo = 1048576;
    }
}
