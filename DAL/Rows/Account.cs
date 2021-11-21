using DAL.Others;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using static DAL.Repos.AccountRepo;

namespace DAL.Rows
{
    public class Account : Row
    {
        public const string DefaultPassword = "1";
        public const int MinPasswordLength = 6;
        public const int MaxPasswordLength = 20;

        public string TaiKhoan { get; set; }
        public string Password { get; set; }
        public string NhomTaiKhoan { get; set; }
        public int DangLogin { get; set; }

        public static Account CreateAccount(string accountName, string accountGroup)
        {
            var account = AccountRepo.Instance.FindByID(new object[] { accountName });
            if (account != null)
                return null;

            account = new Account();
            using (var sh256 = SHA256.Create())
            {
                account.TaiKhoan = accountName;
                var hash = sh256.ComputeHash(Encoding.UTF8.GetBytes(DefaultPassword));
                account.Password = ToHex(ref hash, false);
            }
            account.NhomTaiKhoan = accountGroup;

            return account;
        }

        public Account() { }
        public Account(Account account)
        {
            TaiKhoan = account.TaiKhoan;
            Password = account.Password;
            NhomTaiKhoan = account.NhomTaiKhoan;
            DangLogin = account.DangLogin;
        }

        public void ChangePassword(string password)
        {
            using (var sh256 = SHA256.Create())
            {
                var hash = sh256.ComputeHash(Encoding.UTF8.GetBytes(password));
                Password = ToHex(ref hash, false);
            }
            Save();
        }

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
                        return new ConnectionResult(true, existedToken.Token, existedToken.Bitmask, existedToken.NhanVienID);
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

        public override string Add(UnitOfWork uow = null)
        {
            if (uow == null)
            {
                using (var uowNew = new UnitOfWork())
                {
                    AccountRepo.Instance.Add(this, uowNew);
                    return ExecuteAndReturn(uowNew);
                }
            }

            if (AccountRepo.Instance.Add(this, uow))
                return "";
            else
                return "Failed!";
        }

        public override string Save(UnitOfWork uow = null)
        {
            if (uow == null)
            {
                using (var uowNew = new UnitOfWork())
                {
                    AccountRepo.Instance.Update(new object[] { TaiKhoan }, this, uowNew);
                    return ExecuteAndReturn(uowNew);
                }
            }

            if (AccountRepo.Instance.Update(new object[] { TaiKhoan }, this, uow))
                return "";
            else
                return "Failed!";
        }

        public override string CheckForError()
        {
            try
            {
                if (!IsAccountValid())
                    return "Your account is too short!";

                return "";
            }
            catch (Exception e)
            {
                return $"Unknow error: {e.Message}";
            }
        }

        public bool IsAccountValid()
        {
            if (TaiKhoan.Length < 5)
            {
                return false;
            }

            return true;
        }

        private static string ToHex(ref byte[] bytes, bool upperCase)
        {
            string result = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                string byteStr = bytes[i].ToString(upperCase ? "X2" : "x2");
                foreach (char c in byteStr)
                {
                    result += c;
                }
                bytes[i] = 0;
            }
            return result;
        }
    }
}
