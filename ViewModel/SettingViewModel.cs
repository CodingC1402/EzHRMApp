using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;

namespace ViewModel
{
    public class SettingViewModel : Navigation.ViewModelBase
    {
        public override string ViewName => "Settings";
    }

    public class Theme
    {
        public readonly string ThemeName;
        public readonly Color Accent;

        public readonly Color AccentDarker;
        public readonly Color AccentBrighter;

        public readonly Color AccentDarkest;
        public readonly Color AccentBrightest;

        public readonly Color AccentFaint;

        public readonly Color BackColor;
        public readonly Color BackColorDarker;
        public readonly Color BackColorDarkest;

        public readonly Color ForegroundLight;
        public readonly Color ForegroundDark;
    }
}
