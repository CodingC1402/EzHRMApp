using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos
{
    public class CalculateSalaryRepo : Repo<CalculateSalary>
    {
        public static CalculateSalaryRepo Instance { get; private set; } = new CalculateSalaryRepo();
        private CalculateSalaryRepo()
        {
            TableName = "CACHTINHLUONG";
            PKColsName = new string[]
            {
                "Ten"
            };
        }
    }
}
