using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;
using ViewModel.Helper;

namespace ViewModel
{
    public class SettingViewModel : Navigation.ViewModelBase
    {
        private static SettingViewModel _instance = null;
        public static SettingViewModel Instance { get => _instance; }

        public override string ViewName => "Settings";
        public ListenerEvent<int> OnColorChanged { get; private set; } = new ListenerEvent<int>();

        RelayCommand<int> _changeColorIndexCommand = null;
        public RelayCommand<int> ChangeColorInexCommand => _changeColorIndexCommand ??= new RelayCommand<int>(ExecuteChangeColorIndex);

        public int _currentTheme = 0;
        public int CurrentTheme 
        {
            get => _currentTheme;
            set
            {
                if (_currentTheme == value)
                    return;

                _currentTheme = value;
                OnColorChanged.Invoke(value);
            }
        }

        public void ExecuteChangeColorIndex(int param)
        {
            if (_currentTheme < 0)
                return;

            CurrentTheme = param;
        }

        // Add theme colors
        public SettingViewModel()
        {
            _instance = this;
        }
    }
}
