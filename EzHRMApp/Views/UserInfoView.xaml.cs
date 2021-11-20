using CornControls.CustomControl;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace EzHRMApp.Views
{
    /// <summary>
    /// Interaction logic for PersonalView.xaml
    /// </summary>
    public partial class UserInfoView : UserControl
    {
        private OpenFileDialog openFileDialog;

        public UserInfoView()
        {
            InitializeComponent();
            openFileDialog = new OpenFileDialog();
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
    }
}
