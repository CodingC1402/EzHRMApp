using DAL.Rows;
using MySql.Data.MySqlClient;
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
    public class AccountRepo : Repo<Account, string>
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

        IntPtr unmanagedString = IntPtr.Zero;
        public static AccountRepo Instance { get; private set; } = new AccountRepo();

        private static readonly string _userNameVar = "?UserName";
        private static readonly string _passwordVar = "?LoginPassword";
        private static readonly string _accessTokenVar = "?AccessToken";
        private static readonly string _privilegeMaskVar = "?PrivilegeMask";
        private static readonly string _employeeIDVar = "?StaffID";
        private static readonly string _isLoggedInVar = "?IsLoggedIn";
        private static readonly string _successVar = "?Success";

        private static readonly MySqlCommand cmd = new MySqlCommand { Connection = DatabaseConnector.Connection };

        public override string TableName => "TAIKHOAN";
        public override string IDColName => "TaiKhoan";

        public ConnectionResult Login(string userName, SecureString hashedPassword)
        {
            var account = FindByID(userName);
            if (account != null)
            {
                var result = account.Login(hashedPassword);
                Update(account.TaiKhoan, account);
                return result;
            }

            return new ConnectionResult();
        }

        public bool Logout(string userName)
        {
            var account = FindByID(userName);
            if (account != null)
            {
                var result = account.Logout();
                Update(account.TaiKhoan, account);
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

        public ConnectionResult LoginOld(string userName, SecureString hashedPassword)
        {
            string querry = $@"LOGIN";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = querry;
            cmd.Parameters.Clear();

            cmd.Parameters.AddWithValue(_userNameVar, userName);
            using (SecureString ss = hashedPassword)
            {
                try
                {
                    unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(ss);
                    cmd.Parameters.AddWithValue(_passwordVar, Marshal.PtrToStringUni(unmanagedString));
                }
                finally
                {
                    Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
                }
            }

            cmd.Parameters.AddWithValue(_accessTokenVar, "").Direction = ParameterDirection.Output;
            cmd.Parameters.AddWithValue(_privilegeMaskVar, 0).Direction = ParameterDirection.Output;
            cmd.Parameters.AddWithValue(_employeeIDVar, "").Direction = ParameterDirection.Output;
            cmd.Parameters.AddWithValue(_isLoggedInVar, 0).Direction = ParameterDirection.Output;
            cmd.Parameters.AddWithValue(_successVar, 0).Direction = ParameterDirection.Output;

            try
            {
                DatabaseConnector.OpenConnection();
                cmd.ExecuteNonQuery();

                object value = cmd.Parameters[_privilegeMaskVar].Value;
                var privilegeMask = value != System.DBNull.Value ? (int)value : 0;

                value = cmd.Parameters[_employeeIDVar].Value;
                var employeeID = value != System.DBNull.Value ? (string)value : "";

                value = cmd.Parameters[_accessTokenVar].Value;
                var accessToken = value != System.DBNull.Value ? (string)value : "";

                value = cmd.Parameters[_successVar].Value;
                var success = value != System.DBNull.Value ? (int)value > 0 : false;

                return new ConnectionResult(success, accessToken, privilegeMask, employeeID);
            }
            finally
            {
                DatabaseConnector.CloseConnection();
            }
        }
        public bool LogoutOld(string accessString)
        {
            string querry = $@"LOGOUT";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = querry;
            cmd.Parameters.Clear();

            cmd.Parameters.AddWithValue(_accessTokenVar, accessString);

            try
            {
                DatabaseConnector.OpenConnection();
                cmd.ExecuteNonQuery();
                return true;
            }
            finally
            {
                DatabaseConnector.CloseConnection();
            }
        }
        public bool LogoutAllOld()
        {
            string querry = $@"LOGOUTALL";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = querry;
            cmd.Parameters.Clear();

            try
            {
                DatabaseConnector.OpenConnection();
                cmd.ExecuteNonQuery();
                return true;
            }
            finally
            {
                DatabaseConnector.CloseConnection();
            }
        }
    }
}
