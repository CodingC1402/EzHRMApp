using DAL.Rows;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repos
{
    public class EmployeeRepo : Repo<Employee, string>
    {
        public override string IDColName => "ID";
        public override string TableName => "NHANVIEN";

        public static EmployeeRepo Instance { get; private set; } = new EmployeeRepo();
    }
}
