using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ViewModel
{
    public class HomeViewModel : Navigation.ViewModelBase
    {
        public override string ViewName => "Home";

        public static HomeViewModel Instance { get; private set; }
        public DateTime CurrentTime { get => DateTime.Now; }

        private Timer _currentTimeUpdateTimer = null;

        public HomeViewModel()
        {
            Instance?.CleanUp();
            Instance = this;

            _currentTimeUpdateTimer = new Timer();
            _currentTimeUpdateTimer.Interval = 1000;
            _currentTimeUpdateTimer.Elapsed += (s, e) =>
            {
                RaisePropertyChanged(nameof(CurrentTime));
            };
            _currentTimeUpdateTimer.Start();
        }

        protected override void CleanUp()
        {
            _currentTimeUpdateTimer.Stop();   
        }
    }
}
