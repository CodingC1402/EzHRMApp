using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using static DAL.Repos.AccountRepo;

namespace DAL.Rows
{
    public class Account : Row
    {
        public string TaiKhoan { get; set; }
        public string Password { get; set; }
        public string NhomTaiKhoan { get; set; }
        public int DangLogin { get; set; }

        public ConnectionResult Login(SecureString password)
        {
            IntPtr rawString = IntPtr.Zero;

            try
            {
                rawString = Marshal.SecureStringToGlobalAllocUnicode(password);
                if (Password == Marshal.PtrToStringUni(rawString))
                {
                    if (DangLogin > 0)
                    {
                        var clause = new List<KeyValuePair<string, object>>();
                        clause.Add(new KeyValuePair<string, object>(nameof(AccessToken.Account), TaiKhoan));

                        var existedToken = AccessTokenRepo.Instance.FindBy(clause, false).First();

                        return new ConnectionResult(false, existedToken.Token, existedToken.Bitmask, existedToken.NhanVienID);
                    }

                    DangLogin = 1;
                    var token = AccessTokenRepo.Instance.CreateToken(this);
                    return new ConnectionResult(true, token.Token, token.Bitmask, token.NhanVienID);
                }

                return new ConnectionResult(false, "", 0, "");
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(rawString);
            }
        }

        public bool Logout()
        {
            if (DangLogin == 0)
                return false;

            var tokenRepo = AccessTokenRepo.Instance;
            var token = tokenRepo.FindByAccount(TaiKhoan);
            tokenRepo.Remove(token.Token);

            DangLogin = 0;
            return true;
        }
    }
}
