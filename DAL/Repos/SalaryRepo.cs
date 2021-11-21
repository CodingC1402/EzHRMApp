using System;
using System.Collections.Generic;
using SqlKata.Execution;
using System.Text;
using DAL.Rows;

namespace DAL.Repos
{
    public class SalaryRepo : Repo<Salary>
    {
        public static SalaryRepo Instance { get; private set; } = new SalaryRepo();

        public SalaryRepo()
        {
            TableName = "luong";
            PKColsName = new string[]
            {
                "NgayTinhLuong",
                "IDNhanVien"
            };
        }

        public IEnumerable<Salary> GetAllBetween(DateTime start, DateTime end)
        {
            var db = DatabaseConnector.Database;
            return db.Query(TableName).WhereBetween(PKColsName[0], start, end).Get<Salary>();
        }

        public IEnumerable<Salary> GetByEmployeeBetween(string id, DateTime start, DateTime end)
        {
            var db = DatabaseConnector.Database;
            return db.Query(TableName).Where(PKColsName[1], id).WhereBetween(PKColsName[0], start, end).Get<Salary>();
        }
    }
}
