using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos
{
    public class RoleRepo : Repo<Role, int>
    {
        public override string IDColName => "ID";
        public override string TableName => "CHUCVU";

        public static RoleRepo Instance { get; private set; } = new RoleRepo();
    }
}
