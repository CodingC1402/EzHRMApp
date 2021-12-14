using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.Json;
using ViewModel.Helper;

namespace ViewModel
{
    public class SettingViewModel : Navigation.ViewModelBase
    {
        private static SettingViewModel _instance = null;
        public static SettingViewModel Instance { get => _instance; }

        public override string ViewName => "Setting";
        public ListenerEvent<int> OnColorChanged { get; private set; } = new ListenerEvent<int>();

        RelayCommand<int> _changeColorIndexCommand = null;
        public RelayCommand<int> ChangeColorInexCommand => _changeColorIndexCommand ??= new RelayCommand<int>(ExecuteChangeColorIndex);

        private const string saveLocation = "Setting";

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
            Save();
        }

        public override void OnGetTo()
        {
            base.OnGetTo();
        }

        // Add theme colors
        public SettingViewModel()
        {
            _instance = this;
            var file = Load();
            if (file != null)
            {
                CurrentTheme = file.ColorIndex;
            }
        }

        private SettingFile Load()
        {
            if (File.Exists(saveLocation))
            {
                return JsonSerializer.Deserialize<SettingFile>(File.ReadAllText(saveLocation));
            }

            return null;
        }

        private void Save()
        {
            SettingFile settingFile = new SettingFile
            {
                ColorIndex = CurrentTheme
            };
            string str = JsonSerializer.Serialize(settingFile);
            File.WriteAllText(saveLocation, str);
        }
    }

    [Serializable]
    internal class SettingFile
    {
        public int ColorIndex { get; set; } = 0;
    }
}
