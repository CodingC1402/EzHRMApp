using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos
{
    public class DepartmentRepo : Repo<Department, int>
    {
        public override string IDColName => "ID";
        public override string TableName => "PHONGBAN";

        public static DepartmentRepo Instance { get; private set; } = new DepartmentRepo();
    }
}
