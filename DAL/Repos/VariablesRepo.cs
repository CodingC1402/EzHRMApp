using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Text;
using SqlKata.Execution;

namespace DAL.Repos
{
    public class VariablesRepo : Repo<Variables>
    {
        public static VariablesRepo Instance { get; private set; } = new VariablesRepo();

        public VariablesRepo()
        {
            TableName = "thamso";
            PKColsName = new string[]
            {
                "ThoiDiemTao"
            };
        }

        public Variables GetLatestVariables()
        {
            return DatabaseConnector.Database.Query(TableName).OrderByDesc(PKColsName).Limit(1).First<Variables>();
        }
    }
}
