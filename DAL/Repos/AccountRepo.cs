using DAL.Others;
using DAL.Rows;
using MySql.Data.MySqlClient;
using SqlKata.Execution;
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
    public class AccountRepo : Repo<Account>
    {
        public struct ConnectionResult
        {
            public bool Valid { get; private set; }
            public string AccessToken { get; private set; }
            public int PrivilegesMask { get; private set; }
            public string EmployeeID { get; private set; }

            public ConnectionResult(bool valid, string accesstoken, int privilegeMask, string employeeID)
            {
                Valid = valid;
                AccessToken = accesstoken;
                PrivilegesMask = privilegeMask;
                EmployeeID = employeeID;
            }
        }

        public enum Privileges
        {
            ManageEmployee = 0x000001,
            SearchAndEditEmployee = 0x000002,
            PayRollManagement = 0x000008,
            ManageLeaves = 0x000010,
            SearchAndEditCheckIn = 0x000020,

            CompanyManagement = 0x000100,
            JobsManagement = 0x000200,
            DepartmentManagement = 0x000400,
            ChangeRules = 0x000800,
            SetUpSchedule = 0x001000,
            ManageHoliday = 0x002000,

            Reports = 0x010000,
            AccountGroupManagement = 0x020000,
            AccountManagement = 0x040000,

            // include account info
            PersonalInfo = 0x100000
        }

        public static AccountRepo Instance { get; private set; } = new AccountRepo();
        private AccountRepo()
        {
            TableName = "TAIKHOAN";
            PKColsName = new string[] { "TaiKhoan" };
        }

        public IEnumerable<string> GetAccountGroupsOfActiveEmployees()
        {
            var db = DatabaseConnector.Database;
            return db.Query(TableName)
                    .Select("NhomTaiKhoan")
                    .WhereIn(PKColsName[0],
                        db.Query(EmployeeRepo.Instance.TableName)
                        .Select("TaiKhoan")
                        .Where("NgayThoiViec", null))
                    .Get<string>();
        }

        public ConnectionResult Login(string userName, SecureString hashedPassword)
        {
            var account = FindByID(new object[] { userName });
            if (account != null)
            {
                var result = account.Login(hashedPassword);
                return result;
            }

            return new ConnectionResult();
        }

        public bool Logout(string userName)
        {
            var account = FindByID(new object[] { userName });
            if (account != null)
            {
                var result = account.Logout();
                return result;
            }
            return false;
        }

        public bool LogoutAll()
        {
            var tokenRepo = AccessTokenRepo.Instance;
            var tokens = tokenRepo.GetAll();
            if (tokens.Count() == 0)
                return false;

            foreach (var token in tokens)
            {
                Logout(token.Account);
            }
            return true;
        }
    }
}
