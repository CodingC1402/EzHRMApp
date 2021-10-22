using DAL.Rows;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos
{
    public class AccessTokenRepo : Repo<AccessToken, string>
    {
        public static AccessTokenRepo Instance { get; private set; } = new AccessTokenRepo();
        public override string IDColName => "Token";
        public override string TableName => "ACCESSTOKENS";
        public static int TokenSize { get => 32; }

        public AccessToken FindByAccount(string accountName)
        {
            var db = DatabaseConnector.Database;
            return db.Query(TableName).Where(nameof(AccessToken.Account), accountName).First<AccessToken>();
        }

        public AccessToken CreateToken(Account account)
        {
            string tokenStr = GenerateAccessToken(TokenSize);

            var tokenList = new List<AccessToken>(GetAll());
            bool needReset = true;
            while (needReset)
            {
                needReset = false;
                foreach (var token in tokenList)
                {
                    if (token.Token == tokenStr)
                    {
                        needReset = true;
                    }
                }
            }

            var newToken = new AccessToken
            {
                Token = GenerateAccessToken(TokenSize),
                Account = account.TaiKhoan,
                Bitmask = AccountGroupRepo.Instance.FindByID(account.NhomTaiKhoan).QuyenHan
            };
            Add(newToken);

            return newToken;
        }

        private static Random random = new Random((int)DateTime.Now.Ticks);
        private string GenerateAccessToken(int size)
        {
            StringBuilder stringBuilder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(94 * random.NextDouble() + 32)));
                stringBuilder.Append(ch);
            }

            return stringBuilder.ToString();
        }
    }
}
