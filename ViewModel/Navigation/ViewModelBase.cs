using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace ViewModel.Navigation
{
    [AddINotifyPropertyChangedInterface]
    public abstract class ViewModelBase
    {
        public virtual string ViewName { get => "Base : ^)"; }
    }
}
