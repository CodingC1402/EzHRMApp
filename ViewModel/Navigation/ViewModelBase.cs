using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace ViewModel.Navigation
{
    [AddINotifyPropertyChangedInterface]
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public virtual string ViewName { get => "Base : ^)"; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string propName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public virtual void CleanUp()
        {

        }

        public virtual void OnGetTo()
        {

        }
    }
}
