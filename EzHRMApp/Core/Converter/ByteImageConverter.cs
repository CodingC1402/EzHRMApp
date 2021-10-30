using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EzHRMApp.Core.Converter
{
    using ImageStruct = ViewModel.Structs.Image;
    public class ByteImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(ImageSource))
            {
                return ByteToImage(value as ImageStruct);
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }

        public static ImageSource ByteToImage(ImageStruct imageStruct)
        {
            if (!(imageStruct == null || imageStruct.ImageBytes == null || imageStruct.ImageBytes.Length == 0 || imageStruct.Width == 0))
            {
                try
                {
                    BitmapImage bmpImage = new BitmapImage();
                    MemoryStream ms = new MemoryStream(imageStruct.ImageBytes);
                    bmpImage.BeginInit();
                    bmpImage.StreamSource = ms;
                    bmpImage.EndInit();

                    ImageSource imageSrc = bmpImage as ImageSource;
                    return imageSrc;
                }
                catch
                {}
            }

            return (ImageSource)Application.Current.FindResource("DefaultProfile");
        }
    }
}
