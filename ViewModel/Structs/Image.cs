using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Structs
{
    public class Image
    {
        public Image(Image image)
        {
            ImageBytes = (byte[])image.ImageBytes.Clone();
            Width = image.Width;
            FileType = image.FileType;
        }
        public Image(ProfilePicture profile)
        {
            ImageBytes = profile.Image;
            Width = profile.Width;
            FileType = profile.Type;
        }

        public byte[] ImageBytes { get; set; }
        public int Width { get; set; }
        public string FileType { get; set; }


        public ProfilePicture GetProfile(string id)
        {
            return new ProfilePicture()
            {
                ID = id,
                Image = (byte[])ImageBytes?.Clone(),
                Width = Width,
                Type = FileType
            };
        }
    }
}
