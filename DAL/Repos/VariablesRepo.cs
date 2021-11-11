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

        public static Variables CurrentVariables { get; private set; }

        public VariablesRepo()
        {
            TableName = "thamso";
            PKColsName = new string[]
            {
                "ThoiDiemTao"
            };
            FindLatestVariables();
        }

        public Variables FindLatestVariables()
        {
            return CurrentVariables = DatabaseConnector.Database.Query(TableName).OrderByDesc(PKColsName).Limit(1).First<Variables>();
        }
    }
}
