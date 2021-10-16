using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Security;
using System.Net;
using System.Data;
using MySql.Data.MySqlClient;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace DAL
{
    public class LoginInfo
    {
        public enum Privileges
        {
            SetUpSchedule = 1,
            ManageEmployee = 2,
            ViewEmployee = 4,
            ManagePayroll = 8,
            ManageHoliday = 16,
            CheckIn = 32,
        }

        private static string _accessToken = "";
        private static uint   _privilegeMask = 0;
        private static string _employeeID = "";

        private static readonly string _userNameVar = "?UserName";
        private static readonly string _passwordVar = "?LoginPassword";
        private static readonly string _accessTokenVar = "?AccessToken";
        private static readonly string _privilegeMaskVar = "?PrivilegeMask";
        private static readonly string _employeeIDVar = "?StaffID";
        private static readonly string _isLoggedInVar = "?IsLoggedIn";
        private static readonly string _successVar = "?Success";
        private static readonly MySqlCommand cmd = new MySqlCommand { Connection = DatabaseConnector.Connection };

        public static bool Login(string userName, SecureString password)
        {
            bool success = true;
            using (var sh256 = SHA256.Create())
            {
                byte[] hash;
                IntPtr unmanagedString = IntPtr.Zero;
                try
                {
                    unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(password);
                    hash = sh256.ComputeHash(Encoding.UTF8.GetBytes(Marshal.PtrToStringUni(unmanagedString)));
                }
                finally
                {
                    Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
                }

                bool isLoggedIn = false;
                string querry = $@"LOGIN";

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = querry;
                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue(_userNameVar, userName);
                using (SecureString ss = ToHex(ref hash, false))
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

                cmd.Parameters.AddWithValue(_accessTokenVar, _accessToken).Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue(_privilegeMaskVar, _privilegeMask).Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue(_employeeIDVar, _employeeID).Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue(_isLoggedInVar, isLoggedIn).Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue(_successVar, success).Direction = ParameterDirection.Output;

                try
                {
                    DatabaseConnector.OpenConnection();
                    cmd.ExecuteNonQuery();

                    object value = cmd.Parameters[_privilegeMaskVar].Value;
                    _privilegeMask = value != System.DBNull.Value ? (uint)value : 0;

                    value = cmd.Parameters[_employeeIDVar].Value;
                    _employeeID = value != System.DBNull.Value ? (string)value : "";

                    value = cmd.Parameters[_accessTokenVar].Value;
                    _accessToken = value != System.DBNull.Value ? (string)value : "";
                }
                finally
                {
                    DatabaseConnector.CloseConnection();
                }
            }

            return success;
        }

        public static void Logout()
        {
            string querry = $@"LOGOUT";

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = querry;
            cmd.Parameters.Clear();

            cmd.Parameters.AddWithValue(_accessTokenVar, _accessToken);

            try
            {
                DatabaseConnector.OpenConnection();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                DatabaseConnector.CloseConnection();
            }
        }

        // Using this will also change the bytes to 0 to remove trace 
        private static SecureString ToHex(ref byte[] bytes, bool upperCase)
        {
            SecureString result = new SecureString();
            for (int i = 0; i < bytes.Length; i++)
            {
                string byteStr = bytes[i].ToString(upperCase ? "X2" : "x2");
                foreach (char c in byteStr)
                {
                    result.AppendChar(c);
                }
                bytes[i] = 0;
            }
            return result;
        }
    }
}
