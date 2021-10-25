using DAL.Rows;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos
{
    public class EmployeeRepo : Repo<Employee>
    {
        private static readonly int idFrontLength = 2;
        private static readonly int idNumberLength = 8;

        public static EmployeeRepo Instance { get; private set; } = new EmployeeRepo();
        private EmployeeRepo()
        {
            TableName = "NHANVIEN";
            PKColsName = new string[] { "ID" };
        }

        public string GetNextID()
        {
            var db = DatabaseConnector.Database;
            var employee = db.Query(TableName).OrderByDesc(nameof(Employee.ID)).Limit(1).First<Employee>();
            var nextID = "";
            if (employee != null)
            {
                int currentID = int.Parse(employee.ID.Substring(idFrontLength));
                currentID++;
                nextID = $"NV{currentID.ToString().PadLeft(idNumberLength, '0')}";
            }
            else
            {
                nextID = $"NV{"1".PadLeft(idNumberLength, '0')}";
            }

            return nextID;
        }
    }
}
