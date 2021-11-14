using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace ViewModel.Helper
{
    [AddINotifyPropertyChangedInterface]
    public class PopUpMessage : INotifyPropertyChanged
    {
        private static PopUpMessage _instance = null;
        public static PopUpMessage Instance { get => _instance ??= new PopUpMessage(); }

        private string _message = "";
        private bool _isOpened = false;

        public string Message {
            get => _message;
            set
            {
                _message = value;
                RaisePropertyChanged();
            }
        }
        public bool IsOpened {
            get => _isOpened;
            set
            {
                _isOpened = value;
                RaisePropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string propName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
