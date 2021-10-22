using DAL.Others;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Rows
{
    public class Row
    {
        public virtual bool Save(UnitOfWork uow) { return true; }
    }
}
