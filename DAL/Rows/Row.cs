using DAL.Others;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using PropertyChanged;

namespace DAL.Rows
{
    [AddINotifyPropertyChangedInterface]
    public class Row : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual bool Save(UnitOfWork uow) { return true; }
    }
}
