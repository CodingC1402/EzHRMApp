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
        public virtual string Save(UnitOfWork uow) {
            throw new Exception("Function add from Row base class haven't been implmented or still calling base.Save()");
        }

        public virtual string Add(UnitOfWork uow = null)
        {
            throw new Exception("Function add from Row base class haven't been implmented or still calling base.Add()");
        }

        public virtual string CheckForError()
        {
            return "";
        }

        public string BoolToString(bool value)
        {
            if (value)
                return "";
            else
                return "Failed!";
        }

        public string ExecuteAndReturn(UnitOfWork uow)
        {
            var error = CheckForError();
            if (error != "")
            {
                return error;
            }
            else if (!uow.Complete())
            {
                return "Unknow error occured!";
            }
            else
            {
                return "";
            }
        }
    }
}
