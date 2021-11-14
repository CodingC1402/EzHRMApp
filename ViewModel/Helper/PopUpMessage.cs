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
        private string _title = "";
        public string Message {
            get => _message;
            set
            {
                _message = value;
                RaisePropertyChanged();
                OnPropertyChanged(new EventArgs());
            }
        }
        public bool IsOpened {
            get => _isOpened;
            set
            {
                _isOpened = value;
                RaisePropertyChanged();
                OnPropertyChanged(new EventArgs());
            }
        }
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged();
                OnPropertyChanged(new EventArgs());
            }
        }

        private event EventHandler _whenPropertyChanged = null;
        public event EventHandler WhenPropertyChanged
        {
            add { _whenPropertyChanged += value; }
            remove { _whenPropertyChanged -= value; }
        }

        protected void OnPropertyChanged(EventArgs e)
        {
            if (_whenPropertyChanged != null)
                _whenPropertyChanged(this, e);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string propName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
