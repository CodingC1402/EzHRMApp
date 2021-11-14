using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DAL.Others;

namespace DAL.Repos
{
    // Alittle bit special, have to do this because blob is stupid
    public class ProfilePictureRepo
    {
        public const string ImagesLocation = "Profiles";
        public static ProfilePictureRepo Instance { get; private set; } = new ProfilePictureRepo();

        ProfilePictureRepo()
        {
            if (!Directory.Exists(ImagesLocation))
                Directory.CreateDirectory(ImagesLocation);
        }

        public ProfilePicture FindByID(string employeeID)
        {
            foreach (string file in Directory.GetFiles(ImagesLocation))
            {
                if (Path.GetFileNameWithoutExtension(file) == employeeID)
                {
                    return JsonUtils.DeserialLize<ProfilePicture>(file);
                }
            }

            return null;
        }

        public string Save(ProfilePicture profilePicture)
        {
            if (JsonUtils.Serialize(profilePicture, Path.Combine(ImagesLocation, profilePicture.ID)))
            {
                return "";
            }
            else return "Failed to save image!";
        }

        public string Add(ProfilePicture profilePicture)
        {
            // There are 2 function but they are the same because IO stream is smarter than db : P
            return Save(profilePicture);
        }
    }
}
