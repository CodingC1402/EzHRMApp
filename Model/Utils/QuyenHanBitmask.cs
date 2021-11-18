using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Utils
{
    static class QuyenHanBitmask
    {
        public static int Dashboard = 1;
        public static int CheckIn = 2;
        public static int Schedule = 4;
        public static int Staff = 8;
        public static int Departments = 16;
        public static int Positions = 32;
        public static int AccountGroups = 64;
        public static int Payroll = 128;
        public static int Reports = 256;
    }
}
