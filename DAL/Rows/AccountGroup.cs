﻿using DAL.Others;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Rows
{
    public class AccountGroup : Row
    {
        public string TenNhomTaiKhoan { get; set; }
        public int QuyenHan { get; set; }

        public override bool Save(UnitOfWork uow)
        {
            return AccountGroupRepo.Instance.Update(new object[] { TenNhomTaiKhoan }, this, uow);
        }
    }
}
