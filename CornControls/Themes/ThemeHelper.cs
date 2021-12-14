using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using System.IO;

namespace CornControls.Themes
{
    public static class ThemeHelper
    {
        public enum ThemeColor
        {
            Blue,
            Brown,
            Gray,
            Teal,
            SteelBlue
        }

        private static readonly string FilePath;
        private const string colorSchemePath = "ColorScheme";
        private static ResourceDictionary _colorSchemeResource = new ResourceDictionary();
        private static ResourceDictionary _currentColorScheme = null;
        private static ThemeColor _currentThemeColor = ThemeColor.Blue;

        static ThemeHelper()
        {
            FilePath = Path.Combine(Directory.GetCurrentDirectory(), "Themes");
            FindMainResource();
        }

        public static void SelectTheme(ThemeColor themeColor)
        {
            if (themeColor == _currentThemeColor)
                return;

            var dicts = Application.Current.Resources;
            _ = dicts.MergedDictionaries.Remove(_colorSchemeResource);

            _colorSchemeResource = Clone(_colorSchemeResource);
            _currentColorScheme = LoadResourceDict(Path.Combine(FilePath, $"{themeColor}.xaml"));
            _colorSchemeResource.MergedDictionaries.Clear();
            _colorSchemeResource.MergedDictionaries.Add(_currentColorScheme);

            dicts.MergedDictionaries.Add(_colorSchemeResource);
            _currentThemeColor = themeColor;
        }

        private static ResourceDictionary LoadResourceDict(string path)
        {
            return (ResourceDictionary)XamlReader.Load(System.Xml.XmlReader.Create(path));
        }

        private static ResourceDictionary Clone(ResourceDictionary rd)
        {
            ResourceDictionary cloneRd = new ResourceDictionary();
            foreach(var entry in rd.Keys)
            {
                cloneRd.Add(entry, rd[entry]);
            }
            foreach (var mergedDict in rd.MergedDictionaries)
            {
                cloneRd.MergedDictionaries.Add(mergedDict);
            }

            cloneRd.Source = rd.Source;

            return cloneRd;
        }

        private static void FindMainResource()
        {
            var dicts = Application.Current.Resources.MergedDictionaries;

            Uri resourcePath = new Uri($"/CornControls;component/Themes/{colorSchemePath}.xaml", UriKind.Relative);
            foreach (var dict in dicts)
            {
                if (dict.Source.Equals(resourcePath))
                {
                    _colorSchemeResource = dict;
                    return;
                }
            }
        }
    }
}
