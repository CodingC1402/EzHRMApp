using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos
{
    public class PenaltyRepo : Repo<Penalty>
    {
        private static PenaltyRepo _instance;
        public static PenaltyRepo Instance { get => _instance ??= new PenaltyRepo(); }

        private PenaltyRepo()
        {
            TableName = "TRULUONG";
            PKColsName = new string[] { "ID" };
        }

        public IEnumerable<Penalty> GetEmployeeDeductions(Employee employee, DateTime start, DateTime end)
        {
            var deductions = GetAll();
            var result = new List<Penalty>();
            foreach (Penalty deduct in deductions)
            {
                if (deduct.IDNhanVien == employee.ID && deduct.Ngay >= start && deduct.Ngay < end)
                {
                    result.Add(deduct);
                }
            }

            return result;
        }
    }
}
