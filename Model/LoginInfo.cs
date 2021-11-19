using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Security;
using System.Net;
using System.Data;
using System.Runtime.InteropServices;
using System.Diagnostics;
using DAL.Repos;
using PropertyChanged;

namespace Model
{
    [AddINotifyPropertyChangedInterface]
    public class LoginInfo
    {
        public static int PrivilegeMask
        {
            get => _privilegeMask;
        }
        public static string AccessToken
        {
            get => _accessToken;
        }
        public static string EmployeeID
        {
            get => _employeeID;
        }
        public static EmployeeModel Employee 
        {
            get => _employee;
            set => _employee = value;
        }


        private static string _accessToken = "";
        private static int _privilegeMask = 0;
        private static string _employeeID = "";
        private static string _username = "";
        private static EmployeeModel _employee = null;

        public static uint MinPasswordLength { get => 1; }
        public static uint MaxPasswordLength { get => 16; }

        public static bool Login(string userName, SecureString password)
        {
            using (var sh256 = SHA256.Create())
            {
                bool success = true;
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

                var result = AccountRepo.Instance.Login(userName, ToHex(ref hash, false));
                if (result.Valid)
                {
                    _accessToken = result.AccessToken;
                    _privilegeMask = result.PrivilegesMask;
                    _employeeID = result.EmployeeID;
                    _username = userName;
                }

                UpdateEmployee();
                if (_employee != null && _employee.NgayThoiViec.HasValue)
                {
                    return false;
                }

                success = result.Valid;
                return success;
            }
        }

        public static void UpdateEmployee()
        {
            var employee = EmployeeRepo.Instance.FindByID(new object[] { EmployeeID });
            if (employee != null)
                Employee = new EmployeeModel(employee);
            else
                Employee = null;
        }

        public static void Logout()
        {
            AccountRepo.Instance.Logout(_username);
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
