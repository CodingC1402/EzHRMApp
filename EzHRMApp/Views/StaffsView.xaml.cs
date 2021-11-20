using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CornControls.CustomControl;
using Microsoft.Win32;
using Model;
using PropertyChanged;

namespace EzHRMApp.Views
{
    /// <summary>
    /// Interaction logic for StaffsView.xaml
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class StaffsView : UserControl
    {
        public static readonly DependencyProperty IDWidthProperty = DependencyProperty.Register(nameof(IDWidth), typeof(GridLength), typeof(StaffsView), new PropertyMetadata(new GridLength(100)));
        public static readonly DependencyProperty SurnameWidthProperty = DependencyProperty.Register(nameof(SurnameWidth), typeof(GridLength), typeof(StaffsView), new PropertyMetadata(new GridLength(100)));
        public static readonly DependencyProperty NameWidthProperty = DependencyProperty.Register(nameof(NameWidth), typeof(GridLength), typeof(StaffsView), new PropertyMetadata(new GridLength(100)));
        public static readonly DependencyProperty EmailWidthProperty = DependencyProperty.Register(nameof(EmailWidth), typeof(GridLength), typeof(StaffsView), new PropertyMetadata(new GridLength(100)));
        public static readonly DependencyProperty PhoneNumberWidthProperty = DependencyProperty.Register(nameof(PhoneNumberWidth), typeof(GridLength), typeof(StaffsView), new PropertyMetadata(new GridLength(100)));
        public static readonly DependencyProperty StartWorkingWidthProperty = DependencyProperty.Register(nameof(StartWorkingWidth), typeof(GridLength), typeof(StaffsView), new PropertyMetadata(new GridLength(1, GridUnitType.Star)));

        private OpenFileDialog openFileDialog;

        public GridLength IDWidth {
            get => (GridLength)GetValue(IDWidthProperty);
            set => SetValue(IDWidthProperty, value);
        }
        public GridLength SurnameWidth {
            get => (GridLength)GetValue(SurnameWidthProperty);
            set => SetValue(SurnameWidthProperty, value);
        }
        public GridLength NameWidth {
            get => (GridLength)GetValue(NameWidthProperty);
            set => SetValue(NameWidthProperty, value);
        }
        public GridLength EmailWidth {
            get => (GridLength)GetValue(EmailWidthProperty);
            set => SetValue(EmailWidthProperty, value);
        }
        public GridLength PhoneNumberWidth {
            get => (GridLength)GetValue(PhoneNumberWidthProperty);
            set => SetValue(PhoneNumberWidthProperty, value);
        }
        public GridLength StartWorkingWidth {
            get => (GridLength)GetValue(StartWorkingWidthProperty);
            set => SetValue(StartWorkingWidthProperty, value);
        }

        public StaffsView()
        {
            InitializeComponent();
            openFileDialog = new OpenFileDialog();
            UpdateFilter();
        }

        public void SetFilterForFileDialog()
        {
            openFileDialog.Filter = "";
            openFileDialog.Multiselect = false;
        }

        private void profileBtn_Click(object sender, RoutedEventArgs e)
        {
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var imageSource = new ImageSourceConverter().ConvertFromString("file://" + openFileDialog.FileName) as ImageSource;
                    BitmapSource bitmap = (BitmapSource)imageSource;
                    PngBitmapEncoder pngEncoder = new();
                    pngEncoder.Frames.Add(BitmapFrame.Create(bitmap));

                    FileInfo file = new(openFileDialog.FileName);
                    using (MemoryStream stream = new())
                    {
                        pngEncoder.Save(stream);
                        ViewModel.Structs.Image imageStruct = new()
                        {
                            FileType = file.Extension,
                            ImageBytes = stream.ToArray(),
                            Width = bitmap.PixelWidth
                        };
                        profileBtn.CommandParameter = imageStruct;
                    }
                }
                catch
                {
                    PopUpMessage.ShowErrorMessage("ERROR!", "The file you choose is either not a supported image format or not an image!");
                    profileBtn.CommandParameter = null;
                }
            }
            else
            {
                profileBtn.CommandParameter = null;
            }
        }

        protected void searchChanged(object sender, RoutedEventArgs e)
        {
            UpdateFilter();
        }

        protected virtual void UpdateFilter()
        {
            if (datagridEx.SearchText == "" && showResignCheckBox.IsChecked.HasValue && showResignCheckBox.IsChecked.Value)
            {
                datagridEx.SetCollectionFilter(null);
            }
            else
            {
                datagridEx.SetCollectionFilter(obj =>
                {
                    EmployeeModel employee = obj as EmployeeModel;
                    if ((!showResignCheckBox.IsChecked.HasValue || !showResignCheckBox.IsChecked.Value) && employee.NgayThoiViec.HasValue)
                    {
                        return false;
                    }

                    if (datagridEx.SearchText != "")
                    {
                        var searchText = datagridEx.SearchText;
                        switch (datagridEx.SearchFilter)
                        {
                            case "ID":
                                return employee.ID.Contains(searchText);
                            case "First name":
                                return employee.Ten.Contains(searchText);
                            case "Surname":
                                return employee.Ho.Contains(searchText);
                            case "Phone number":
                                return employee.SDTVanPhong.Contains(searchText);
                            case "Email":
                                return employee.EmailVanPhong.Contains(searchText);
                        }
                    }

                    return true;
                });
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void PercentValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        // Clamp value
        private void TextBoxEx_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            float value = 0;
            bool isFloat = float.TryParse(textBox.Text, out value);
            if (!isFloat)
            {
                textBox.Text = "0";
            }
            else
            {
                if (value > 1)
                {
                    textBox.Text = "1";
                }
                else if (value < 0)
                {
                    textBox.Text = "0";
                }
            }
        }
    }
}
