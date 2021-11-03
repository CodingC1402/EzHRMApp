using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos
{
    public class DepartmentRepo : Repo<Department>
    {
        public static DepartmentRepo Instance { get; private set; } = new DepartmentRepo();
        private DepartmentRepo()
        {
            TableName = "PHONGBAN";
            PKColsName = new string[] { "TenPhong" };
        }
    }
}
