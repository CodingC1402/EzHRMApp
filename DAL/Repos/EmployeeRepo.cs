using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos
{
    public class EmployeeRepo : Repo<Employee>
    {
        public static EmployeeRepo Instance { get; private set; } = new EmployeeRepo();
        private EmployeeRepo()
        {
            TableName = "NHANVIEN";
            PKColsName = new string[] { "ID" };
        }
    }
}
