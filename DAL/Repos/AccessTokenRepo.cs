using DAL.Others;
using DAL.Rows;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos
{
    public class AccessTokenRepo : Repo<AccessToken>
    {
        public static AccessTokenRepo Instance { get; private set; } = new AccessTokenRepo();
        private AccessTokenRepo()
        {
            TableName = "ACCESSTOKENS";
            PKColsName = new string[] { "Token" };

        }
        public static int TokenSize { get => 32; }

        public AccessToken FindByAccount(string accountName)
        {
            var db = DatabaseConnector.Database;
            return db.Query(TableName).Where(nameof(AccessToken.Account), accountName).First<AccessToken>();
        }

        public AccessToken CreateToken(Account account, UnitOfWork uow)
        {
            string tokenStr = GenerateAccessToken(TokenSize);

            if (!uow.Repos.ContainsKey(TableName))
            {
                uow.Repos.Add(TableName, new List<Row>(GetAll()));
            }
            bool needReset = true;
            while (needReset)
            {
                needReset = false;
                foreach (AccessToken token in uow.Repos[TableName])
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
                Bitmask = AccountGroupRepo.Instance.FindByID(new object[] { account.NhomTaiKhoan }).QuyenHan
            };
            Add(newToken, uow);

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
