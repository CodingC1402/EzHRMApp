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

        static ThemeHelper()
        {
            FilePath = Path.Combine(Directory.GetCurrentDirectory(), "Themes");
            FindMainResource();
        }

        public static void SelectTheme(ThemeColor themeColor)
        {
            var dicts = Application.Current.Resources;

            Uri resourcePath = new Uri($"/CornControls;component/Themes/{colorSchemePath}.xaml", UriKind.Relative);
            foreach (var dict in dicts.MergedDictionaries)
            {
                if (dict.Source.Equals(resourcePath))
                {
                    _colorSchemeResource = dict;
                    break;
                }
            }

            dicts.MergedDictionaries.Remove(_colorSchemeResource);
            _currentColorScheme = LoadResourceDict(Path.Combine(FilePath, $"{themeColor}.xaml"));
            _colorSchemeResource.MergedDictionaries.Clear();
            _colorSchemeResource.MergedDictionaries.Add(_currentColorScheme);

            dicts.MergedDictionaries.Add(_colorSchemeResource);
        }

        private static ResourceDictionary LoadResourceDict(string path)
        {
            return (ResourceDictionary)XamlReader.Load(System.Xml.XmlReader.Create(path));
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
