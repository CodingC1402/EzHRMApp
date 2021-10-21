using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos
{
    public class EmployeeRepo : Repo<Employee, string>
    {
        public override string IDColName => "ID";
        public override string TableName => "NHANVIEN";
    }
}
