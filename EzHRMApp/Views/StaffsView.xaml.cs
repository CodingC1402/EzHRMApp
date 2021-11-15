using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
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
        private bool viewingReport = false;

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
            reportView.Visibility = Visibility.Collapsed;
            infoView.Visibility = Visibility.Visible;
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

        private void showResignedEmployeesChanged(object sender, RoutedEventArgs e)
        {
            UpdateFilter();
        }
        private void filterTypeChanged(object sender, RoutedEventArgs e)
        {
            UpdateFilter();
        }
        private void filterTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateFilter();
        }

        private void UpdateFilter()
        {
            ICollectionView cv = CollectionViewSource.GetDefaultView(datagridEx.ItemsSource);
            if (filterTextBox.Text == "" && showResignCheckBox.IsChecked.HasValue && showResignCheckBox.IsChecked.Value)
            {
                cv.Filter = null;
            }
            else
            {
                cv.Filter = obj =>
                {
                    EmployeeModel employee = obj as EmployeeModel;
                    if ((!showResignCheckBox.IsChecked.HasValue || !showResignCheckBox.IsChecked.Value) && employee.NgayThoiViec.HasValue)
                    {
                        return false;
                    }

                    if (filterTextBox.Text != "")
                    {
                        var searchText = filterTextBox.Text;
                        switch ((filterComboboxs.SelectedItem as DataGridTextColumn).Header)
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
                };
            }
        }

        private void changeModeBtn_Click(object sender, RoutedEventArgs e)
        {
            viewingReport = !viewingReport;
            if (viewingReport)
            {
                viewLabel.Text = "Report";
                changeModeBtn.Content = "View Infos";
                addPenaltyBtn.Visibility = reportView.Visibility = Visibility.Visible;
                changeOrCancleBtn.Visibility = newOrConfirmBtn.Visibility = infoView.Visibility = Visibility.Collapsed;
            }
            else
            {
                viewLabel.Text = "Infos";
                changeModeBtn.Content = "View Report";
                addPenaltyBtn.Visibility = reportView.Visibility = Visibility.Collapsed;
                changeOrCancleBtn.Visibility = newOrConfirmBtn.Visibility = infoView.Visibility = Visibility.Visible;
            }
        }
    }
}
