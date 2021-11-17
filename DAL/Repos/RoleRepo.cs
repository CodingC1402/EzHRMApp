using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos
{
    public class RoleRepo : Repo<Role>
    {
        public static RoleRepo Instance { get; private set; } = new RoleRepo();
        private RoleRepo()
        {
            TableName = "CHUCVU";
            PKColsName = new string[] {
                "TenChucVu"
            };
        }

    }
}
