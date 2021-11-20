using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos
{
    public class PenaltyTypeRepo : Repo<PenaltyType>
    {
        private static PenaltyTypeRepo _instance;
        public static PenaltyTypeRepo Instance { get => _instance ??= new PenaltyTypeRepo(); }

        private PenaltyTypeRepo()
        {
            TableName = "CACLOAIVIPHAM";
            PKColsName = new string[] { "TenViPham" };
        }
    }
}
