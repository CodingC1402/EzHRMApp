using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Security;
using System.Net;

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

        private string _accessToken = "";
        private uint _privilegeMask = 0;

        public static void Login(string userName, string password)
        {
            using (var sh256 = SHA256Managed.Create())
            {
                var hash = sh256.ComputeHash(Encoding.Default.GetBytes(userName));
                using (SecureString ss = new NetworkCredential("", ToHex(ref hash, false)).SecurePassword)
                {
                    
                }
            }


        }

        // Using this will also change the bytes to 0 to remove trace 
        private static string ToHex(ref byte[] bytes, bool upperCase)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);
            for (int i = 0; i < bytes.Length; i++)
            {
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));
                bytes[i] = 0;
            }
            return result.ToString();
        }
    }
}
