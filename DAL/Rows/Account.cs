using DAL.Others;
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
            UnitOfWork uow = new UnitOfWork();

            try
            {
                rawString = Marshal.SecureStringToGlobalAllocUnicode(password);
                if (Password == Marshal.PtrToStringUni(rawString))
                {
                    if (DangLogin > 0)
                    {
                        var existedToken = AccessTokenRepo.Instance.FindBy(nameof(AccessToken.Account), TaiKhoan).First();
                        return new ConnectionResult(false, existedToken.Token, existedToken.Bitmask, existedToken.NhanVienID);
                    }

                    DangLogin = 1;
                    Save(uow);
                    var token = AccessTokenRepo.Instance.CreateToken(this, uow);

                    if (uow.Complete())
                        return new ConnectionResult(true, token.Token, token.Bitmask, token.NhanVienID);
                }

                return new ConnectionResult(false, "", 0, "");
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(rawString);
                uow.Dispose();
            }
        }

        public bool Logout()
        {
            if (DangLogin == 0)
                return false;

            using (UnitOfWork uow = new UnitOfWork())
            {
                DangLogin = 0;
                var tokenRepo = AccessTokenRepo.Instance;
                var token = tokenRepo.FindByAccount(TaiKhoan);

                tokenRepo.Remove(new object[] { token.Token }, uow);
                Save(uow);

                uow.Complete();
            }
            return true;
        }

        public override bool Save(UnitOfWork uow)
        {
            return AccountRepo.Instance.Update(new object[] { TaiKhoan }, this, uow);
        }
    }
}
