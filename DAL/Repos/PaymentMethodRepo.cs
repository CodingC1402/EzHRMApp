using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos
{
    public class PaymentMethodRepo : Repo<PaymentMethod>
    {
        public static PaymentMethodRepo Instance { get; private set; } = new PaymentMethodRepo();
        private PaymentMethodRepo()
        {
            TableName = "CACHTINHLUONG";
            PKColsName = new string[]
            {
                "Ten"
            };
        }
    }
}
